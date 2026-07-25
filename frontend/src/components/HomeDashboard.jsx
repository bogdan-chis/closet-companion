import GarmentShelf from "./GarmentShelf";
import OutfitCard, { isCompleted } from "./OutfitCard";

export default function HomeDashboard({ garments, outfits, onToggleFavorite, onGoToWardrobe, onGoToGenerate }) {
  const completedOutfits = outfits.filter((o) => isCompleted(o.status));
  const favoriteOutfits = completedOutfits.filter((o) => o.isFavorite);

  return (
    <div className="home-dashboard">
      <div className="toolbar">
        <span className="count-label">
          {completedOutfits.length} {completedOutfits.length === 1 ? "ținută generată" : "ținute generate"}
        </span>
        <div className="toolbar-actions">
          <button className="btn-add" onClick={onGoToWardrobe}>Garderoba</button>
          <button className="btn-submit btn-generate" onClick={onGoToGenerate}>Generează Ținută</button>
        </div>
      </div>

      <section className="home-section">
        <h2 className="section-title">Ținute favorite</h2>
        {favoriteOutfits.length === 0 ? (
          <div className="empty-state"><p>Nicio ținută favorită încă</p></div>
        ) : (
          <div className="outfit-grid">
            {favoriteOutfits.map((o) => (
              <OutfitCard key={o.id} outfit={o} onToggleFavorite={onToggleFavorite} />
            ))}
          </div>
        )}
      </section>

      <section className="home-section">
        <h2 className="section-title">Piesele tale</h2>
        <GarmentShelf garments={garments} />
      </section>

      <section className="home-section">
        <h2 className="section-title">Toate ținutele</h2>
        {completedOutfits.length === 0 ? (
          <div className="empty-state"><p>Nicio ținută generată încă — încearcă combinații din garderobă</p></div>
        ) : (
          <div className="outfit-grid">
            {completedOutfits.map((o) => (
              <OutfitCard key={o.id} outfit={o} onToggleFavorite={onToggleFavorite} />
            ))}
          </div>
        )}
      </section>
    </div>
  );
}