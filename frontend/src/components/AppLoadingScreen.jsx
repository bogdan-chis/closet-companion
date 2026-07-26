import { useEffect, useState } from "react";

const STAGES = [
  "Se deschide dulapul...",
  "Se numără piesele...",
  "Se aranjează ținutele...",
  "Aproape gata...",
];

export default function AppLoadingScreen() {
  const [stageIndex, setStageIndex] = useState(0);
  const [showColdStartNote, setShowColdStartNote] = useState(false);

  useEffect(() => {
    const stageInterval = setInterval(() => {
      setStageIndex((i) => (i + 1) % STAGES.length);
    }, 2600);

    // Server is likely just waking up from sleep — say so, don't leave her guessing.
    const coldStartTimer = setTimeout(() => setShowColdStartNote(true), 8000);

    return () => {
      clearInterval(stageInterval);
      clearTimeout(coldStartTimer);
    };
  }, []);

  return (
    <div className="app-loading-screen">
      <svg className="loader-svg" viewBox="0 0 160 200" xmlns="http://www.w3.org/2000/svg">
        <defs>
          <clipPath id="garmentClipApp">
            <path d="M55 40 L65 28 L95 28 L105 40 L120 55 L108 70 L100 62 L100 175 L60 175 L60 62 L52 70 L40 55 Z" />
          </clipPath>
        </defs>
        <line x1="80" y1="14" x2="80" y2="26" className="loader-line" />
        <path d="M80 26 Q80 18 90 20 Q100 24 80 32 Q60 24 70 20 Q80 18 80 26" className="loader-line" fill="none" />
        <path
          d="M55 40 L65 28 L95 28 L105 40 L120 55 L108 70 L100 62 L100 175 L60 175 L60 62 L52 70 L40 55 Z"
          className="loader-garment-outline"
        />
        <g clipPath="url(#garmentClipApp)">
          <rect x="0" y="0" width="160" height="26" className="loader-scan" />
        </g>
      </svg>

      <div className="loader-text-wrap">
        <p key={stageIndex} className="loader-stage-text">{STAGES[stageIndex]}</p>
      </div>

      <div className="loader-dots">
        <span /><span /><span />
      </div>

      {showColdStartNote && (
        <p className="cold-start-note">
          Bogdan a ațipit un pic — se trezește acum, mai durează câteva secunde 😴
        </p>
      )}
    </div>
  );
}