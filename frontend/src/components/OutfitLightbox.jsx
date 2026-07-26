import { useEffect } from "react";

export default function OutfitLightbox({ outfit, onClose, onToggleFavorite, onDelete }) {
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

  if (!outfit) return null;

  return (
    <div className="lightbox-backdrop" onClick={onClose}>
      <div className="lightbox-content" onClick={(e) => e.stopPropagation()}>
        <button className="lightbox-close" onClick={onClose} aria-label="Închide">×</button>

        <img className="lightbox-image" src={outfit.resultImageUrl} alt="Ținută generată" />

        <div className="lightbox-footer">
          <p className="lightbox-date">
            {new Date(outfit.generatedOn).toLocaleDateString("ro-RO", {
              day: "numeric", month: "long", year: "numeric",
            })}
          </p>

          <div className="lightbox-actions">
            <button
              className={`heart-btn lightbox-heart ${outfit.isFavorite ? "heart-active" : ""}`}
              onClick={() => onToggleFavorite(outfit.id)}
              aria-label={outfit.isFavorite ? "Elimină din favorite" : "Adaugă la favorite"}
            >
              {outfit.isFavorite ? "♥" : "♡"}
            </button>

            {onDelete && (
              <button
                className="btn-add lightbox-delete"
                onClick={() => { onDelete(outfit.id); onClose(); }}
              >
                Șterge ținuta
              </button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}