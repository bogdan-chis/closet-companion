import { useEffect, useState } from "react";
import { getGarments, deleteGarment } from "./api/garments";
import { getPoses, createPose } from "./api/poses";
import GarmentForm from "./components/GarmentForm";
import GarmentList from "./components/GarmentList";
import PoseGallery from "./components/PoseGallery";
import OutfitBuilder from "./components/OutfitBuilder";
import PasswordGate from "./components/PasswordGate";
import "./App.css";

const EMPTY_SELECTION = { top: null, bottom: null, dress: null, shoes: null };

function App() {
  const [unlocked, setUnlocked] = useState(
    () => sessionStorage.getItem("catalina-closet-unlocked") === "true"
  );

  // "wardrobe" | "outfit-builder" | "fitting-room"
  const [view, setView] = useState("wardrobe");

  const [garments, setGarments] = useState([]);
  const [poses, setPoses] = useState([]);
  const [selectedPoseId, setSelectedPoseId] = useState(null);
  const [outfitSelection, setOutfitSelection] = useState(EMPTY_SELECTION);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);

  const [showComingSoon, setShowComingSoon] = useState(false);

  async function loadData() {
    try {
      const [garmentsData, posesData] = await Promise.all([getGarments(), getPoses()]);

      setGarments(garmentsData);
      setPoses(posesData);

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

  if (!unlocked) {
    return <PasswordGate onUnlock={() => setUnlocked(true)} />;
  }

  const count = garments.length;
  const countLabel = count === 1 ? "1 piesă în garderobă" : `${count} piese în garderobă`;

  const hasAnySelection = Object.values(outfitSelection).some(Boolean);

  return (
    <div className="app-shell">
      <header className="masthead">
        <h1
          className="wordmark"
          onClick={() => setView("wardrobe")}
          style={{ cursor: "pointer" }}
        >
          Garderoba Cătălinei
        </h1>
        <p className="tagline">Hainele ei preferate, într-un singur loc</p>
      </header>

      {view === "wardrobe" && (
        <>
          <div className="toolbar">
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
            <button className="btn-add" onClick={() => setView("wardrobe")}>
              ← Garderobă
            </button>
            <span className="count-label">Alege piesele pentru ținută</span>
          </div>

          {!loading && !error && (
            <OutfitBuilder
              garments={garments}
              selection={outfitSelection}
              onChangeSelection={setOutfitSelection}
            />
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
      <button className="btn-add" onClick={() => setView("outfit-builder")}>
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
      <>
        <button
          className="btn-submit btn-generate"
          style={{ marginTop: "1.5rem" }}
          onClick={() => setShowComingSoon(true)}
        >
          ✨ Generează Ținuta
        </button>

        {showComingSoon && (
          <div className="coming-soon-card">
            <p className="coming-soon-title">Aproape gata! 🪄</p>
            <p className="coming-soon-text">
              Generarea automată a ținutei e încă în lucru — următorul pas
              este să facem magia să prindă viață aici. Piesele și
              ipostaza ta sunt deja salvate, așa că totul e pregătit
              pentru momentul în care va fi gata.
            </p>
            <button className="coming-soon-close" onClick={() => setShowComingSoon(false)}>
              Am înțeles
            </button>
          </div>
        )}
      </>
    )}
  </div>
)}
    </div>
  );
}

export default App;