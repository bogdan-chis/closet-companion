export default function ConfirmDialog({ open, title, message, onConfirm, onCancel }) {
  if (!open) return null;

  return (
    <div className="confirm-backdrop" onClick={onCancel}>
      <div className="confirm-card" onClick={(e) => e.stopPropagation()}>
        <h3 className="confirm-title">{title}</h3>
        <p className="confirm-message">{message}</p>
        <div className="confirm-actions">
          <button className="btn-add confirm-cancel" onClick={onCancel}>Anulează</button>
          <button className="confirm-delete" onClick={onConfirm}>Șterge</button>
        </div>
      </div>
    </div>
  );
}