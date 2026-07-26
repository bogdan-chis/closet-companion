const CATEGORY_LABELS = ["Top", "Bottom", "Rochie", "Pantofi"];

export default function GarmentList({ garments, onOpen }) {
  if (garments.length === 0) {
    return (
      <div className="empty-state">
        <p>Niciun articol încă — adaugă primul articol mai sus</p>
      </div>
    );
  }

  return (
    <div className="garment-grid">
      {garments.map((garment) => (
        <div key={garment.id} className="garment-card">
          <div
            className="garment-image-wrap"
            onClick={() => onOpen(garment)}
            role="button"
            tabIndex={0}
          >
            <img className="garment-image" src={garment.imageUrl} alt={garment.name} />
          </div>
          <h3 className="garment-name">{garment.name}</h3>
          <p className="garment-ref">Ref — {CATEGORY_LABELS[garment.category] ?? garment.category}</p>
        </div>
      ))}
    </div>
  );
}