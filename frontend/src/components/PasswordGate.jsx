import { useState } from "react";
import "./PasswordGate.css";

const WRONG_MESSAGES = [
  "Nu-i asta — încearcă din nou 🍋",
  "Aproape... dar nu-i lămâie. 🍋",
  "Prea acru, nu-i parola bună!",
  "Nu te supăra, mai încearcă o dată!",
  "Greșit! Lămâia rămâne necojită.",
];

export default function PasswordGate({ onUnlock }) {
  const [value, setValue] = useState("");
  const [error, setError] = useState(null);
  const [shake, setShake] = useState(false);

  function handleSubmit(e) {
    e.preventDefault();
    if (value.trim().toLowerCase() === "lemon") {
      sessionStorage.setItem("catalina-closet-unlocked", "true");
      onUnlock();
    } else {
      setError(WRONG_MESSAGES[Math.floor(Math.random() * WRONG_MESSAGES.length)]);
      setShake(true);
      setTimeout(() => setShake(false), 450);
    }
  }

  return (
    <div className="gate-screen">
      <div className={`gate-card ${shake ? "shake" : ""}`}>
        <div className="gate-mascot" aria-hidden="true">
          <svg viewBox="0 0 200 200" width="120" height="120">
            <ellipse cx="100" cy="105" rx="78" ry="68" fill="#FFD639" />
            <ellipse cx="45" cy="95" rx="10" ry="14" fill="#F4C430" transform="rotate(-20 45 95)" />
            <ellipse cx="155" cy="95" rx="10" ry="14" fill="#F4C430" transform="rotate(20 155 95)" />
            <circle cx="75" cy="100" r="7" fill="#1A1A1A" />
            <circle cx="125" cy="100" r="7" fill="#1A1A1A" />
            <path d="M75 130 Q100 148 125 130" stroke="#1A1A1A" strokeWidth="5" fill="none" strokeLinecap="round" />
            <circle cx="60" cy="120" r="8" fill="#FF9AA2" opacity="0.6" />
            <circle cx="140" cy="120" r="8" fill="#FF9AA2" opacity="0.6" />
            <path d="M95 40 Q100 15 115 25 Q108 40 95 40Z" fill="#6FA25B" />
          </svg>
        </div>

        <h1 className="gate-title">Garderoba Catalinei</h1>
        <p className="gate-subtitle">
          Dulapul ăsta e încuiat! 🍋<br />Știi parola?
        </p>

        <form className="gate-form" onSubmit={handleSubmit}>
          <input
            type="password"
            className="gate-input"
            placeholder="Parola secretă..."
            value={value}
            onChange={(e) => setValue(e.target.value)}
            autoFocus
          />
          <button type="submit" className="gate-button">Deschide dulapul 🍋</button>
        </form>

        {error && <p className="gate-error">{error}</p>}

        <p className="gate-hint">Psst... e ceva galben, acru și rotund 👀</p>
      </div>
    </div>
  );
}