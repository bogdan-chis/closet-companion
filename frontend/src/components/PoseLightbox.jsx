import { useEffect } from "react";

export default function PoseLightbox({ pose, onClose, onDelete, onSelect, isSelected }) {
  useEffect(() => {
    function handleKey(e) {
      if (e.key === "Escape") onClose();
    }
    document.addEventListener("keydown", handleKey);
    document.body.style.overflow = "hidden";
    return () => {
      document.removeEventListener("keydown", handleKey);
      document.body.style.overflow = "";
    };
  }, [onClose]);

  if (!pose) return null;

  return (
    <div className="lightbox-backdrop" onClick={onClose}>
      <div className="lightbox-content" onClick={(e) => e.stopPropagation()}>
        <button className="lightbox-close" onClick={onClose} aria-label="Închide">×</button>

        <img className="lightbox-image" src={pose.imageUrl} alt={pose.name} />

        <div className="lightbox-footer">
          <div>
            <p className="lightbox-garment-name">{pose.name}</p>
            <p className="lightbox-date">{isSelected ? "✓ Selectată" : "Nu e selectată"}</p>
          </div>

          <div className="lightbox-actions">
            {!isSelected && (
              <button
                className="btn-add"
                onClick={() => { onSelect(pose); onClose(); }}
              >
                Selectează
              </button>
            )}
            {onDelete && (
              <button
                className="btn-add lightbox-delete"
                onClick={() => { onDelete(pose.id); onClose(); }}
              >
                Șterge ipostaza
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}