import CarouselRow from "./CarouselRow";

const CATEGORY = { TOP: 0, BOTTOM: 1, DRESS: 2 };

export default function OutfitBuilder({ garments, selection, onChangeSelection }) {
  const byCategory = (id) => garments.filter((g) => g.category === id);
  const dressActive = Boolean(selection.dress);

  function handleSelect(key, item) {
    const next = { ...selection, [key]: item ? item.id : null };

    if (key === "dress" && item) {
      next.top = null;
      next.bottom = null;
    }
    if ((key === "top" || key === "bottom") && item) {
      next.dress = null;
    }

    onChangeSelection(next);
  }

  return (
    <div className="outfit-builder">
      <CarouselRow
        categoryId={CATEGORY.TOP}
        items={byCategory(CATEGORY.TOP)}
        selectedId={selection.top}
        onSelect={(item) => handleSelect("top", item)}
        disabled={dressActive}
      />
      <CarouselRow
        categoryId={CATEGORY.BOTTOM}
        items={byCategory(CATEGORY.BOTTOM)}
        selectedId={selection.bottom}
        onSelect={(item) => handleSelect("bottom", item)}
        disabled={dressActive}
      />
      <CarouselRow
        categoryId={CATEGORY.DRESS}
        items={byCategory(CATEGORY.DRESS)}
        selectedId={selection.dress}
        onSelect={(item) => handleSelect("dress", item)}
      />
    </div>
  );
}