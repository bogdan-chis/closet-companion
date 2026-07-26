import { useEffect } from "react";

const CATEGORY_LABELS = ["Top", "Bottom", "Rochie", "Pantofi"];

export default function GarmentLightbox({ garment, onClose, onDelete }) {
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

  if (!garment) return null;

  return (
    <div className="lightbox-backdrop" onClick={onClose}>
      <div className="lightbox-content" onClick={(e) => e.stopPropagation()}>
        <button className="lightbox-close" onClick={onClose} aria-label="Închide">×</button>

        <img className="lightbox-image" src={garment.imageUrl} alt={garment.name} />

        <div className="lightbox-footer">
          <div>
            <p className="lightbox-garment-name">{garment.name}</p>
            <p className="lightbox-date">
              Ref — {CATEGORY_LABELS[garment.category] ?? garment.category}
            </p>
          </div>

          {onDelete && (
            <button
              className="btn-add lightbox-delete"
              onClick={() => { onDelete(garment.id); onClose(); }}
            >
              Șterge articolul
            </button>
          )}
        </div>
      </div>
    </div>
  );
}