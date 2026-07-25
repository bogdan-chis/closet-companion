const isCompleted = (s) => s === 2 || s === "Completed";

export default function OutfitCard({ outfit, onToggleFavorite }) {
  return (
    <div className="outfit-card">
      <div className="outfit-card-image-wrap">
        <img className="outfit-card-image" src={outfit.resultImageUrl} alt="Ținută generată" />
        <button
          className={`heart-btn ${outfit.isFavorite ? "heart-active" : ""}`}
          onClick={() => onToggleFavorite(outfit.id)}
          aria-label={outfit.isFavorite ? "Elimină din favorite" : "Adaugă la favorite"}
        >
          {outfit.isFavorite ? "♥" : "♡"}
        </button>
      </div>
      <p className="outfit-card-date">
        {new Date(outfit.generatedOn).toLocaleDateString("ro-RO")}
      </p>
    </div>
  );
}

export { isCompleted };