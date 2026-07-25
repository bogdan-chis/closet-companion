import { useEffect, useState } from "react";
import { getGarments, deleteGarment } from "./api/garments";
import { getPoses, createPose } from "./api/poses";
import { generateOutfit, getGeneratedOutfit, getAllOutfits, toggleFavoriteOutfit } from "./api/outfits";
import GarmentForm from "./components/GarmentForm";
import GarmentList from "./components/GarmentList";
import PoseGallery from "./components/PoseGallery";
import OutfitBuilder from "./components/OutfitBuilder";
import PasswordGate from "./components/PasswordGate";
import HomeDashboard from "./components/HomeDashboard";
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
  const [isFormOpen, setIsFormOpen] = useState(false);

  const [generationId, setGenerationId] = useState(null);
  const [generationStatus, setGenerationStatus] = useState(null);
  const [generatedImageUrl, setGeneratedImageUrl] = useState(null);
  const [generationError, setGenerationError] = useState(null);
  const [isGenerating, setIsGenerating] = useState(false);

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
    await deleteGarment(id);
    setGarments((prev) => prev.filter((g) => g.id !== id));
  }

  function handleCreated(newGarment) {
    setGarments((prev) => [...prev, newGarment]);
    setIsFormOpen(false);
  }

  async function handleAddPose({ imageUrl }) {
    try {
      const newPose = await createPose({
        name: "Poză nouă",
        poseCategory: 0,
        imageUrl,
        isDefault: poses.length === 0,
      });
      setPoses((prev) => [...prev, newPose]);
      setSelectedPoseId(newPose.id);
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
    const garmentIds = Object.values(outfitSelection).filter(Boolean);
    if (garmentIds.length === 0 || !selectedPoseId) return;

    setIsGenerating(true);
    setGenerationError(null);
    setGeneratedImageUrl(null);

    try {
      const result = await generateOutfit(selectedPoseId, garmentIds);
      setGenerationId(result.id);
      setGenerationStatus(result.status);
    } catch (err) {
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
      <header className="masthead">
        <h1 className="wordmark" onClick={() => setView("home")} style={{ cursor: "pointer" }}>
          Garderoba Cătălinei
        </h1>
        <p className="tagline">Hainele ei preferate, într-un singur loc</p>
      </header>

      {view === "home" && (
        <HomeDashboard
          garments={garments}
          outfits={outfits}
          onToggleFavorite={handleToggleFavorite}
          onGoToWardrobe={() => setView("wardrobe")}
          onGoToGenerate={() => setView("outfit-builder")}
        />
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
                  <div className="spinner" />
                  <p>Se generează ținuta... poate dura până la un minut.</p>
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
    </div>
  );
}

export default App;