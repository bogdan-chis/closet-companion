import { useState, useEffect } from "react";

const CATEGORY_LABELS = { 0: "Top", 1: "Bottom", 2: "Dress" };

export default function CarouselRow({ categoryId, items, selectedId, onSelect, disabled }) {
  // slides[0] is always the "empty" option
  const slides = [null, ...items];
  const findIndex = () => {
    const i = selectedId ? slides.findIndex((s) => s && s.id === selectedId) : 0;
    return i === -1 ? 0 : i;
  };
  const [index, setIndex] = useState(findIndex);

  useEffect(() => {
    setIndex(findIndex());
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedId, items.length]);

  function goTo(nextIndex) {
    if (disabled) return;
    const clamped = (nextIndex + slides.length) % slides.length;
    setIndex(clamped);
    onSelect(slides[clamped]);
  }

  const current = slides[index];

  return (
    <div className={`carousel-row ${disabled ? "carousel-disabled" : ""}`}>
      <div className="carousel-header">
        <h3 className="carousel-title">{CATEGORY_LABELS[categoryId]}</h3>
        {disabled && <span className="carousel-note">Dezactivat — ai ales o rochie</span>}
      </div>

      {items.length === 0 ? (
        <div className="carousel-empty">
          <p>Nicio piesă în această categorie încă.</p>
        </div>
      ) : (
        <>
          <div className="carousel-track">
            <button
              type="button"
              className="carousel-arrow"
              onClick={() => goTo(index - 1)}
              disabled={disabled}
              aria-label={`${CATEGORY_LABELS[categoryId]} anterior`}
            >
              ‹
            </button>

            <div className="carousel-slide">
              {current ? (
                <>
                  <div className="carousel-image-wrap">
                    <img className="carousel-image" src={current.imageUrl} alt={current.name} />
                  </div>
                  <p className="carousel-item-name">{current.name}</p>
                </>
              ) : (
                <div className="carousel-empty-slide">
                  <span>Niciuna</span>
                </div>
              )}
            </div>

            <button
              type="button"
              className="carousel-arrow"
              onClick={() => goTo(index + 1)}
              disabled={disabled}
              aria-label={`${CATEGORY_LABELS[categoryId]} următor`}
            >
              ›
            </button>
          </div>

          <p className="carousel-status">
            {current ? "✓ Selectat" : "Nesetat"}
          </p>
        </>
      )}
    </div>
  );
}