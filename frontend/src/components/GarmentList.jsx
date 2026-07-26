const CATEGORY_LABELS = ["Top", "Bottom", "Dress"];

export default function GarmentList({ garments, onDelete }) {
  if (garments.length === 0) {
    return (
      <div className="empty-state">
        <p>Nicio piesă încă — adaugă prima piesă mai sus</p>
      </div>
    );
  }

  return (
    <div className="garment-grid">
      {garments.map((garment) => (
        <div key={garment.id} className="garment-card">
          <div className="garment-image-wrap">
            <img className="garment-image" src={garment.imageUrl} alt={garment.name} />
          </div>
          <button
            className="garment-remove"
            onClick={() => onDelete(garment.id)}
            aria-label={`Șterge ${garment.name}`}
          >
            Șterge
          </button>
          <h3 className="garment-name">{garment.name}</h3>
          <p className="garment-ref">Ref — {CATEGORY_LABELS[garment.category] ?? garment.category}</p>
        </div>
      ))}
    </div>
  );
}