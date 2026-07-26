import { useState } from "react";
import { uploadFile } from "../api/storage";
import PoseLightbox from "./PoseLightbox";

export default function PoseGallery({ poses, onSelectPose, onAddPose, selectedPoseId, onDeletePose }) {
  const [isAdding, setIsAdding] = useState(false);
  const [name, setName] = useState("");
  const [category, setCategory] = useState("0");
  const [file, setFile] = useState(null);
  const [openPoseId, setOpenPoseId] = useState(null);

  const [isUploading, setIsUploading] = useState(false);
  const [error, setError] = useState(null);

  async function handleSubmit(e) {
    e.preventDefault();
    if (!file) {
      setError("Alege o imagine pentru a continua.");
      return;
    }

    setIsUploading(true);
    setError(null);

    try {
      const imageUrl = await uploadFile(file, "poses");
      await onAddPose({ name, poseCategory: Number(category), imageUrl });

      setName("");
      setCategory("0");
      setFile(null);
      setIsAdding(false);
    } catch (err) {
      setError(err.message);
    } finally {
      setIsUploading(false);
    }
  }

  const openPose = poses.find((p) => p.id === openPoseId) || null;

  return (
    <div className="pose-gallery-section">
      <h2 className="form-title">Ipostazele Tale</h2>

      {error && <p className="form-error">{error}</p>}

      {poses.length === 0 ? (
        <div className="empty-state" style={{ marginBottom: "2rem" }}>
          <p>Nicio ipostază disponibilă. Adaugă o fotografie de bază pentru a putea proba hainele.</p>
        </div>
      ) : (
        <div className="garment-grid" style={{ marginBottom: "2rem" }}>
          {poses.map((pose) => (
            <div
              key={pose.id}
              className="garment-card"
              onClick={() => onSelectPose(pose)}
              style={{ cursor: "pointer" }}
            >
              <div
                className="garment-image-wrap"
                style={{
                  border: selectedPoseId === pose.id ? "2px solid var(--color-ink)" : "none",
                  padding: selectedPoseId === pose.id ? "4px" : "0",
                  transition: "all 0.2s ease",
                }}
              >
                <img className="garment-image" src={pose.imageUrl} alt={pose.name} />

                <button
                  className="pose-expand-btn"
                  onClick={(e) => {
                    e.stopPropagation(); // don't trigger card selection
                    setOpenPoseId(pose.id);
                  }}
                  aria-label={`Vezi detalii ${pose.name}`}
                >
                  ⤢
                </button>
              </div>
              <h3 className="garment-name" style={{ textAlign: "center", marginBottom: "0.2rem" }}>
                {selectedPoseId === pose.id ? "✓ Selectat" : "Alege"}
              </h3>
              <p className="garment-ref" style={{ textAlign: "center" }}>
                {pose.name}
              </p>
            </div>
          ))}
        </div>
      )}

      {!isAdding ? (
        <button
          className="btn-add"
          onClick={() => setIsAdding(true)}
          style={{ display: "inline-block", textAlign: "center" }}
        >
          + Adaugă o poză nouă
        </button>
      ) : (
        <form className="form-panel" onSubmit={handleSubmit}>
          <h3 className="form-title">Ipostază nouă</h3>

          <div className="field-grid">
            <div className="field">
              <label htmlFor="pose-name">Descriere</label>
              <input
                id="pose-name"
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="ex. Lumina naturală, Față"
                required
              />
            </div>

            <div className="field">
              <label htmlFor="pose-category">Tip Ipostază</label>
              <select
                id="pose-category"
                value={category}
                onChange={(e) => setCategory(e.target.value)}
                required
              >
                <option value="0">Full Body</option>
                <option value="1">Upper Body</option>
                <option value="2">Lower Body</option>
                <option value="3">Other</option>
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

          <div style={{ display: "flex", gap: "1rem" }}>
            <button type="submit" className="btn-submit" disabled={isUploading}>
              {isUploading ? "Se încarcă…" : "Salvează ipostaza"}
            </button>
            <button
              type="button"
              className="btn-add"
              onClick={() => setIsAdding(false)}
              style={{ background: "transparent", color: "var(--color-ink)", width: "100%" }}
              disabled={isUploading}
            >
              Anulează
            </button>
          </div>
        </form>
      )}

      <PoseLightbox
        pose={openPose}
        onClose={() => setOpenPoseId(null)}
        onDelete={onDeletePose}
        onSelect={onSelectPose}
        isSelected={openPose ? selectedPoseId === openPose.id : false}
      />
    </div>
  );
}