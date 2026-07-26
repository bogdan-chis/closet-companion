import { useEffect, useState } from "react";

const STAGES = [
  "Conving pixelii să coopereze politicos...",
  "Analizez implicațiile filozofice ale acestei ținute...",
  "Calculez fizica cuantică a asortării culorilor...",
  "Calc cu grijă o cută digitală foarte încăpățânată...",
  "Îndepărtez o ultimă scamă virtuală...",
  "Consult consiliul secret al croitorilor AI...",
  "Dezbat cu un algoritm dacă dungile orizontale chiar măresc...",
  "Redimensionez buzunarele pentru a încăpea mai mult context...",
  "Aplic un filtru invizibil de eleganță absolută...",
  "Măsor de trei ori, generez o singură dată...",
  "Verific dacă șosetele se asortează (chiar dacă nu se văd)...",
  "Negociez cu rețeaua neurală pentru o textură mai fină...",
  "Trec materialul prin teste riguroase de aerodinamică...",
  "Ajustez nivelul de încredere în sine pe care îl emană croiala...",
  "Adaug un strop de «je ne sais quoi» complet sintetic...",
];

export default function GenerationLoader() {
  // Initialize with a random stage right off the bat
  const [stageIndex, setStageIndex] = useState(() => 
    Math.floor(Math.random() * STAGES.length)
  );

  useEffect(() => {
    const interval = setInterval(() => {
      setStageIndex((currentIndex) => {
        let nextIndex;
        // Keep picking a random number until it's different from the current one
        // This prevents the same text from appearing twice in a row
        do {
          nextIndex = Math.floor(Math.random() * STAGES.length);
        } while (nextIndex === currentIndex);
        
        return nextIndex;
      });
    }, 3200);
    
    return () => clearInterval(interval);
  }, []);

  return (
    <div className="loader-scene">
      <svg className="loader-svg" viewBox="0 0 160 200" xmlns="http://www.w3.org/2000/svg">
        <defs>
          <clipPath id="garmentClip">
            <path d="M55 40 L65 28 L95 28 L105 40 L120 55 L108 70 L100 62 L100 175 L60 175 L60 62 L52 70 L40 55 Z" />
          </clipPath>
        </defs>

        {/* Hanger */}
        <line x1="80" y1="14" x2="80" y2="26" className="loader-line" />
        <path d="M80 26 Q80 18 90 20 Q100 24 80 32 Q60 24 70 20 Q80 18 80 26" className="loader-line" fill="none" />

        {/* Garment outline */}
        <path
          d="M55 40 L65 28 L95 28 L105 40 L120 55 L108 70 L100 62 L100 175 L60 175 L60 62 L52 70 L40 55 Z"
          className="loader-garment-outline"
        />

        {/* Sweeping scan line, clipped to the garment shape */}
        <g clipPath="url(#garmentClip)">
          <rect x="0" y="0" width="160" height="26" className="loader-scan" />
        </g>
      </svg>

      <div className="loader-text-wrap">
        <p key={stageIndex} className="loader-stage-text">{STAGES[stageIndex]}</p>
      </div>

      <div className="loader-dots">
        <span />
        <span />
        <span />
      </div>
    </div>
  );
}