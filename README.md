# EcoVital — .NET MAUI Mobile App (Final Project)

Cross-platform **.NET MAUI** app that helps track eco items and basic recycling guidance.  
Originally deployed on **Azure** (API + DB). This repo includes a **local mock API** so it runs **without any cloud accounts**.

---

## Demo (30–45s)
1) Login with `demo / demo`.
2) List eco items, open details, create a local record.
3) Trigger a mock **/sync** call.

> The goal is to show app structure and engineering practices; cloud resources are intentionally decoupled.

---

## Tech Stack
- **App:** .NET MAUI (C#), MVVM  
- **Data:** SQLite (offline-first) + `HttpClient` for REST  
- **Services:** Auth, Items, Sync  
- **Docs:** DocFX (only config tracked; generated site ignored)

---

## Run Locally (no Azure)

### 1) Start the mock API (Node)
Requires Node.js.

From repo root:
    npm i -g json-server
    json-server --watch db.json --port 3001

Endpoints exposed by the mock:
- `POST /login` → `{ "token": "demo-token-123" }`
- `GET /ecoItems`
- `GET /ecoItems/:id`
- `POST /sync`

Base URL used by the app: `http://localhost:3001`

### 2) Build the app
Run:
    dotnet restore
    dotnet build
    # Launch on Android emulator or Windows target

---

## Architecture (high level)
- **MVVM** (Models / Views / ViewModels)  
- **Repository pattern** over SQLite for offline cache  
- **API client** with retries/backoff & cancellation tokens  
- **Error handling**: network/timeout → user feedback

Project layout:
    EcoVital_App/
     ├─ EcoVital/                 # .NET MAUI app
     │  ├─ Models/
     │  ├─ Views/
     │  ├─ ViewModels/
     │  ├─ Services/              # AuthService, EcoItemsService, SyncService
     │  ├─ Data/                  # SQLite & repositories (if present)
     │  └─ AppShell.xaml, MainPage.xaml, MauiProgram.cs, ...
     ├─ UnitTestsEcoVital/        # unit tests (if present)
     ├─ db.json                   # local mock API data
     ├─ docfx.json                # docs config (generated site is ignored)
     └─ EcoVital.sln

---

## License
MIT. See `LICENSE`.