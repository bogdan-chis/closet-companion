# Closet Companion 👗✨
Closet Companion is a personalized, full-stack web application designed as a digital wardrobe and virtual try-on assistant. Built as a custom gift, it allows the user to catalog her clothes, mix-and-match outfits, and use AI to generate realistic images of herself wearing the selected garments.

To ensure API efficiency and a seamless user experience, the app includes a "Favorites" feature where successfully generated outfits are permanently saved for future reference.

## 🌟 Core Features
Digital Wardrobe: A catalog of clothing items containing images from the original stores, categorized by type (tops, bottoms, dresses, etc.).

Virtual Fitting Room: The ability to select a base photo and layer multiple clothing items to create a complete outfit.

AI-Powered Try-On: Integration with the Nano Banana AI API to fuse the base photo and clothing items into a realistic, high-quality generated image.

Outfit Gallery: A database-backed gallery that saves favorite generated outfits and their original source images, eliminating redundant API costs for previously generated looks.

Accessible Anywhere: Cloud-deployed architecture allowing the user to access her virtual closet from any device via a secure link.

## 🛠️ Technology Stack
- Frontend: React (Building a responsive, mobile-friendly user interface).

- Backend: C# / ASP.NET Core Web API (Handling secure business logic and API key management).

- Database & Storage: Supabase (PostgreSQL for relational data, Supabase Storage for hosting user and clothing images).

- AI Integration: Nano Banana API (Handling the image generation).

- Hosting: Azure App Service (Backend) & Vercel (Frontend).

## 🏗️ Architecture
The backend enforces a streamlined Domain-Driven Design (DDD) approach, structured strictly into the following layers:

- Domain/: Core business entities, value objects, and domain enums.

- Repository/: Data access layer handling communication with the Supabase PostgreSQL database.

- Service/: Core business logic, use cases, and external integrations (e.g., calling the AI API).

- Controller/: The presentation layer handling HTTP requests and responses for the React frontend.
