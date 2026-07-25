import { useEffect, useState } from "react";
import { getGarments, deleteGarment } from "./api/garments";
import GarmentForm from "./components/GarmentForm";
import GarmentList from "./components/GarmentList";
import PasswordGate from "./components/PasswordGate";
import "./App.css";

function App() {
  const [unlocked, setUnlocked] = useState(
    () => sessionStorage.getItem("catalina-closet-unlocked") === "true"
  );
  const [garments, setGarments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);

  async function loadGarments() {
    try {
      const data = await getGarments();
      setGarments(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    if (unlocked) loadGarments();
  }, [unlocked]);

  async function handleDelete(id) {
    await deleteGarment(id);
    setGarments((prev) => prev.filter((g) => g.id !== id));
  }

  function handleCreated(newGarment) {
    setGarments((prev) => [...prev, newGarment]);
    setIsFormOpen(false);
  }

  if (!unlocked) {
    return <PasswordGate onUnlock={() => setUnlocked(true)} />;
  }

  const count = garments.length;
  const countLabel = count === 1 ? "1 piesă în garderobă" : `${count} piese în garderobă`;

  return (
    <div className="app-shell">
      <header className="masthead">
        <h1 className="wordmark">Garderoba Cătălinei</h1>
        <p className="tagline">Hainele ei preferate, într-un singur loc</p>
      </header>

      <div className="toolbar">
        <span className="count-label">{loading ? "Se încarcă…" : countLabel}</span>
        <button className="btn-add" onClick={() => setIsFormOpen((v) => !v)}>
          {isFormOpen ? "Închide" : "+ Adaugă piesă"}
        </button>
      </div>

      {isFormOpen && <GarmentForm onGarmentCreated={handleCreated} />}

      {error && <p className="form-error">{error}</p>}

      {!loading && !error && <GarmentList garments={garments} onDelete={handleDelete} />}
    </div>
  );
}

export default App;