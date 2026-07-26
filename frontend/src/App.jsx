import { useEffect, useState } from "react";
import { getGarments, deleteGarment } from "./api/garments";
import { getPoses, createPose, deletePose } from "./api/poses";
import { generateOutfit, getGeneratedOutfit, getAllOutfits, toggleFavoriteOutfit, deleteOutfit } from "./api/outfits";
import { getCredits } from "./api/client";
import GarmentForm from "./components/GarmentForm";
import GarmentList from "./components/GarmentList";
import PoseGallery from "./components/PoseGallery";
import OutfitBuilder from "./components/OutfitBuilder";
import PasswordGate from "./components/PasswordGate";
import HomeDashboard from "./components/HomeDashboard";
import GenerationLoader from "./components/GenerationLoader";
import "./App.css";

const EMPTY_SELECTION = { top: null, bottom: null, dress: null };
const isCompleted = (s) => s === 2 || s === "Completed";
const isFailed = (s) => s === 3 || s === "Failed";

function App() {
  const [unlocked, setUnlocked] = useState(
    () => sessionStorage.getItem("catalina-closet-unlocked") === "true"
  );

  // "home" | "wardrobe" | "outfit-builder" | "fitting-room"
  const [view, setView] = useState("home");

  const [garments, setGarments] = useState([]);
  const [poses, setPoses] = useState([]);
  const [outfits, setOutfits] = useState([]);
  const [selectedPoseId, setSelectedPoseId] = useState(null);
  const [outfitSelection, setOutfitSelection] = useState(EMPTY_SELECTION);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [successMessage, setSuccessMessage] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);

  const [generationId, setGenerationId] = useState(null);
  const [generationStatus, setGenerationStatus] = useState(null);
  const [generatedImageUrl, setGeneratedImageUrl] = useState(null);
  const [generationError, setGenerationError] = useState(null);
  const [isGenerating, setIsGenerating] = useState(false);

  const [credits, setCredits] = useState(0);

  // Helper to show floating success toast
  function showSuccess(message) {
    setSuccessMessage(message);
    setTimeout(() => setSuccessMessage(null), 3000);
  }

  async function loadData() {
    try {
      const [garmentsData, posesData, outfitsData] = await Promise.all([
        getGarments(),
        getPoses(),
        getAllOutfits(),
      ]);
      setGarments(garmentsData);
      setPoses(posesData);
      setOutfits(outfitsData);

      if (posesData.length > 0) {
        const defaultPose = posesData.find((p) => p.isDefault) || posesData[0];
        setSelectedPoseId(defaultPose.id);
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    if (unlocked) loadData();
  }, [unlocked]);

  useEffect(() => {
    async function loadCredits() {
      try {
        const val = await getCredits();
        setCredits(val);
      } catch (err) {
        console.error("Eroare la încărcarea creditelor:", err);
      }
    }
    loadCredits();
  }, []);

  useEffect(() => {
    if (!generationId) return;
    if (isCompleted(generationStatus) || isFailed(generationStatus)) return;

    const interval = setInterval(async () => {
      try {
        const outfit = await getGeneratedOutfit(generationId);
        setGenerationStatus(outfit.status);

        if (isCompleted(outfit.status)) {
          setGeneratedImageUrl(outfit.resultImageUrl);
          setIsGenerating(false);
          setOutfits((prev) => {
            const exists = prev.some((o) => o.id === outfit.id);
            return exists ? prev.map((o) => (o.id === outfit.id ? outfit : o)) : [...prev, outfit];
          });
        } else if (isFailed(outfit.status)) {
          setGenerationError(outfit.errorMessage || "Generarea a eșuat.");
          setIsGenerating(false);
        }
      } catch (err) {
        setGenerationError(err.message);
        setIsGenerating(false);
      }
    }, 4000);

    return () => clearInterval(interval);
  }, [generationId, generationStatus]);

  async function handleDelete(id) {
    try {
      await deleteGarment(id);
      setGarments((prev) => prev.filter((g) => g.id !== id));
      const updatedOutfits = await getAllOutfits();
      setOutfits(updatedOutfits);
      
      setError(null);
      showSuccess("Piesa a fost ștearsă cu succes!");
    } catch (err) {
      setError("Eroare la ștergerea piesei: " + err.message);
    }
  }

  async function handleDeleteOutfit(id) {
    try {
      await deleteOutfit(id);
      setOutfits((prev) => prev.filter((o) => o.id !== id));
      
      setError(null);
      showSuccess("Ținuta a fost ștearsă cu succes!");
    } catch (err) {
      setError("Eroare la ștergerea ținutei: " + err.message);
    }
  }

  async function handleDeletePose(id) {
    try {
      await deletePose(id);
      
      if (selectedPoseId === id) {
        const remaining = poses.filter((p) => p.id !== id);
        setSelectedPoseId(remaining.length > 0 ? remaining[0].id : null);
      }
      
      setPoses((prev) => prev.filter((p) => p.id !== id));
      
      const updatedOutfits = await getAllOutfits();
      setOutfits(updatedOutfits);

    setError(null);
    showSuccess("Ipostaza a fost ștearsă cu succes!");
    } catch (err) {
      setError("Eroare la ștergerea ipostazei: " + err.message);
    }
  }

  function handleCreated(newGarment) {
    setGarments((prev) => [...prev, newGarment]);
    setIsFormOpen(false);
    showSuccess("Piesa a fost adăugată cu succes!");
  }

  async function handleAddPose({ name, poseCategory, imageUrl }) {
    try {
      const newPose = await createPose({
        name: name,
        poseCategory: poseCategory,
        imageUrl,
        isDefault: poses.length === 0,
      });
      setPoses((prev) => [...prev, newPose]);
      setSelectedPoseId(newPose.id);

      setError(null);
      showSuccess("Ipostaza a fost adăugată cu succes!");
    } catch (err) {
      setError(err.message);
    }
  }

  async function handleToggleFavorite(id) {
    try {
      await toggleFavoriteOutfit(id);
      setOutfits((prev) => prev.map((o) => (o.id === id ? { ...o, isFavorite: !o.isFavorite } : o)));
    } catch (err) {
      setError(err.message);
    }
  }

  function resetGeneration() {
    setGenerationId(null);
    setGenerationStatus(null);
    setGeneratedImageUrl(null);
    setGenerationError(null);
    setIsGenerating(false);
  }

  async function handleGenerate() {
    if (credits <= 0) {
      setGenerationError("Nu mai ai credite disponibile! Generarea este dezactivată.");
      return;
    }

    const garmentIds = Object.values(outfitSelection).filter(Boolean);
    if (garmentIds.length === 0 || !selectedPoseId) return;

    setIsGenerating(true);
    setGenerationError(null);
    setGeneratedImageUrl(null);

    setCredits((prev) => prev - 1);

    try {
      const result = await generateOutfit(selectedPoseId, garmentIds);
      setGenerationId(result.id);
      setGenerationStatus(result.status);
    } catch (err) {
      // Optional: Refund the credit if the API call instantly fails
      setCredits((prev) => prev + 1);
      setGenerationError(err.message);
      setIsGenerating(false);
    }
  }

  if (!unlocked) {
    return <PasswordGate onUnlock={() => setUnlocked(true)} />;
  }

  const count = garments.length;
  const countLabel = count === 1 ? "1 piesă în garderobă" : `${count} piese în garderobă`;
  const hasAnySelection = Object.values(outfitSelection).some(Boolean);

  return (
    <div className="app-shell">
      <div className={`credit-badge ${credits < 5 ? 'credit-danger' : credits < 10 ? 'credit-warning' : ''}`}>
        ⚡ {credits} {credits === 1 ? 'credit' : 'credite'}
      </div>
      <header className="masthead">
        <h1 className="wordmark" onClick={() => setView("home")} style={{ cursor: "pointer" }}>
          Garderoba Cătălinei
        </h1>
        <p className="tagline">Hainele ei preferate, într-un singur loc</p>
      </header>

      {view === "home" && (
        <>
          {error && (
            <p className="form-error" style={{ textAlign: "center" }}>
              {error}
            </p>
          )}
          <HomeDashboard
            garments={garments}
            outfits={outfits}
            onToggleFavorite={handleToggleFavorite}
            onDeleteOutfit={handleDeleteOutfit}
            onGoToWardrobe={() => setView("wardrobe")}
            onGoToGenerate={() => setView("outfit-builder")}
          />
        </>
      )}

      {view === "wardrobe" && (
        <>
          <div className="toolbar">
            <button className="btn-add" onClick={() => setView("home")}>← Acasă</button>
            <span className="count-label">{loading ? "Se încarcă…" : countLabel}</span>
            <div className="toolbar-actions">
              <button className="btn-add" onClick={() => setIsFormOpen((v) => !v)}>
                {isFormOpen ? "Închide" : "+ Adaugă piesă"}
              </button>
              <button className="btn-submit btn-generate" onClick={() => setView("outfit-builder")}>
                Generează Ținută
              </button>
            </div>
          </div>

          {isFormOpen && <GarmentForm onGarmentCreated={handleCreated} />}
          {error && <p className="form-error">{error}</p>}
          {!loading && !error && <GarmentList garments={garments} onDelete={handleDelete} />}
        </>
      )}

      {view === "outfit-builder" && (
        <div className="outfit-builder-view">
          <div className="toolbar">
            <button className="btn-add" onClick={() => setView("home")}>← Acasă</button>
            <span className="count-label">Alege piesele pentru ținută</span>
          </div>

          {!loading && !error && (
            <OutfitBuilder garments={garments} selection={outfitSelection} onChangeSelection={setOutfitSelection} />
          )}

          {hasAnySelection && (
            <button
              className="btn-submit btn-generate"
              style={{ marginTop: "1.5rem" }}
              onClick={() => setView("fitting-room")}
            >
              Continuă la alegerea ipostazei →
            </button>
          )}
        </div>
      )}

      {view === "fitting-room" && (
        <div className="fitting-room-view">
          <div className="toolbar">
            <button className="btn-add" onClick={() => { resetGeneration(); setView("outfit-builder"); }}>
              ← Piese alese
            </button>
            <span className="count-label">Alege o ipostază</span>
          </div>

          {!loading && !error && (
            <PoseGallery
              poses={poses}
              selectedPoseId={selectedPoseId}
              onSelectPose={(pose) => setSelectedPoseId(pose.id)}
              onAddPose={handleAddPose}
              onDeletePose={handleDeletePose}
            />
          )}

          {selectedPoseId && (
            <div className="generation-panel">
              {!isGenerating && !generatedImageUrl && !generationError && (
                <button className="btn-submit btn-generate" onClick={handleGenerate}>
                  ✨ Generează Ținuta
                </button>
              )}

              {isGenerating && (
                <div className="generation-status">
                  <GenerationLoader />
                  <p>Poate dura până la un minut.</p>
                </div>
              )}

              {generatedImageUrl && (
                <div className="generation-result">
                  <img src={generatedImageUrl} alt="Ținută generată" className="generated-image" />
                  <div style={{ display: "flex", gap: "0.75rem" }}>
                    <button className="btn-add" onClick={resetGeneration}>Generează din nou</button>
                    <button className="btn-add" onClick={() => { resetGeneration(); setView("home"); }}>
                      Vezi pe Acasă
                    </button>
                  </div>
                </div>
              )}

              {generationError && (
                <div className="generation-error-card">
                  <p>Ceva nu a mers bine: {generationError}</p>
                  <button className="btn-add" onClick={resetGeneration}>Încearcă din nou</button>
                </div>
              )}
            </div>
          )}
        </div>
      )}

      {/* Global Toast Popup */}
      {successMessage && (
        <div className="toast-popup">
          ✓ {successMessage}
        </div>
      )}
    </div>
  );
}

export default App;