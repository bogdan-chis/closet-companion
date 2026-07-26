export default function GarmentShelf({ garments }) {
  if (garments.length === 0) {
    return (
      <div className="empty-state">
        <p>Niciun articol încă</p>
      </div>
    );
  }

  return (
    <div className="shelf-track">
      {garments.map((g) => (
        <div key={g.id} className="shelf-item">
          <div className="shelf-image-wrap">
            <img className="shelf-image" src={g.imageUrl} alt={g.name} />
          </div>
          <p className="shelf-item-name">{g.name}</p>
        </div>
      ))}
    </div>
  );
}