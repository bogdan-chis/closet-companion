import { useState } from "react";
import { uploadFile } from "../api/storage";
import { createGarment } from "../api/garments";

export default function GarmentForm({ onGarmentCreated }) {
  const [name, setName] = useState("");
  const [category, setCategory] = useState("");
  const [file, setFile] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState(null);

  async function handleSubmit(e) {
    e.preventDefault();
    if (!file) {
      setError("Alege o imagine pentru a continua.");
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      const imageUrl = await uploadFile(file, "garments");
      const garment = await createGarment({
        name,
        category: Number(category),
        imageUrl,
        sourceWebsiteUrl: "",
      });

      onGarmentCreated(garment);

      setName("");
      setCategory("");
      setFile(null);
      e.target.reset();
    } catch (err) {
      setError(err.message);
    } finally {
      setIsSubmitting(false);
    }
  }

  return (
    <form className="form-panel" onSubmit={handleSubmit}>
      <h2 className="form-title">Piesă nouă</h2>

      <div className="field-grid">
        <div className="field">
          <label htmlFor="name">Nume</label>
          <input
            id="name"
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="ex. Rochie de seară"
            required
          />
        </div>

        <div className="field">
          <label htmlFor="category">Categorie</label>
          <select
            id="category"
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            required
          >
            <option value="" disabled>Alege</option>
            <option value="0">Top</option>
            <option value="1">Pantalon</option>
            <option value="2">Rochie</option>
            <option value="3">Geacă</option>
          </select>
        </div>

        <div className="field field-full">
          <label>Imagine</label>
          <div className="dropzone">
            <span className="dropzone-label">
              {file ? "Schimbă fișierul" : "Apasă sau trage o imagine"}
            </span>
            <input
              type="file"
              accept="image/*"
              onChange={(e) => setFile(e.target.files[0])}
              required
            />
            {file && <span className="dropzone-filename">{file.name}</span>}
          </div>
        </div>
      </div>

      {error && <p className="form-error">{error}</p>}

      <button type="submit" className="btn-submit" disabled={isSubmitting}>
        {isSubmitting ? "Se încarcă…" : "Adaugă în garderobă"}
      </button>
    </form>
  );
}