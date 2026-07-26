import { useState } from "react";
import GarmentShelf from "./GarmentShelf";
import OutfitCard, { isCompleted } from "./OutfitCard";
import OutfitLightbox from "./OutfitLightbox";

export default function HomeDashboard({ garments, outfits, onToggleFavorite, onDeleteOutfit, onGoToWardrobe, onGoToGenerate }) {
  const [openOutfit, setOpenOutfit] = useState(null);

  const completedOutfits = outfits.filter((o) => isCompleted(o.status));
  const favoriteOutfits = completedOutfits.filter((o) => o.isFavorite);

  // Keep the lightbox showing fresh data (e.g. heart toggled) while it's open
  const liveOpenOutfit = openOutfit
    ? completedOutfits.find((o) => o.id === openOutfit.id) || null
    : null;

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
              <OutfitCard key={o.id} outfit={o} onToggleFavorite={onToggleFavorite} onOpen={setOpenOutfit} />
            ))}
          </div>
        )}
      </section>

      <section className="home-section">
        <h2 className="section-title">Articolele tale</h2>
        <GarmentShelf garments={garments} />
      </section>

      <section className="home-section">
        <h2 className="section-title">Toate ținutele</h2>
        {completedOutfits.length === 0 ? (
          <div className="empty-state"><p>Nicio ținută generată încă — încearcă combinații din garderobă</p></div>
        ) : (
          <div className="outfit-grid">
            {completedOutfits.map((o) => (
              <OutfitCard
                key={o.id}
                outfit={o}
                onToggleFavorite={onToggleFavorite}
                onOpen={setOpenOutfit}
              />
            ))}
          </div>
        )}
      </section>

      <OutfitLightbox
        outfit={liveOpenOutfit}
        onClose={() => setOpenOutfit(null)}
        onToggleFavorite={onToggleFavorite}
        onDelete={onDeleteOutfit}
      />
    </div>
  );
}