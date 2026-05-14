# Match-Making - Dating App

A full-stack real-time dating platform where users can browse member profiles, connect with matches, send instant messages, and track online presence.

## Tech Stack

### Backend
- **ASP.NET Core 9.0** with C#
- **Entity Framework Core** (SQL Server)
- **ASP.NET Core Identity** (authentication & role management)
- **JWT Bearer** + refresh token authentication
- **SignalR** for real-time messaging & presence
- **Cloudinary** for image hosting

### Frontend
- **Angular 20** with TypeScript
- **Tailwind CSS** + **DaisyUI** for styling
- **Microsoft SignalR client** for real-time features
- **RxJS** for reactive programming

### Infrastructure
- **SQL Server 2022** (via Docker)
- **Cloudinary CDN** for photo storage

## Features

- **Authentication** — Register, login, JWT access tokens with HttpOnly refresh token cookies, role-based authorization (Member, Admin, Moderator)
- **Member Profiles** — Bio, photos, location, last-active tracking
- **Photo Management** — Upload, set main photo, delete (Cloudinary integration)
- **Member Discovery** — Browse members with pagination and filtering
- **Likes** — Like/unlike members, view mutual likes and admirers
- **Real-Time Messaging** — Instant chat via SignalR with read receipts and message history
- **Presence Tracking** — Online/offline status with real-time notifications

## Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS)
- [Docker](https://www.docker.com/) (for SQL Server)

### 1. Start the Database

```bash
docker-compose up -d
```

This starts a SQL Server 2022 container on port `1433`.

### 2. Run the Backend

```bash
cd API
dotnet run
```

The API starts at `https://localhost:5001` (or the port configured in `launchSettings.json`). Database migrations run automatically on startup.

### 3. Run the Frontend

```bash
cd client
npm install
ng serve
```

The Angular app starts at `http://localhost:4200`.

## Project Structure

```
DatingApp/
├── API/                    # ASP.NET Core backend
│   ├── Controllers/        # API endpoints
│   ├── Data/               # DbContext, repositories, migrations, seed data
│   ├── DTOs/               # Data transfer objects
│   ├── Entities/           # Domain models
│   ├── Helpers/            # Pagination, filters, Cloudinary settings
│   ├── Interfaces/         # Repository & service contracts
│   ├── Middleware/         # Exception handling
│   ├── Services/           # Token & photo services
│   └── SignalR/            # MessageHub, PresenceHub, PresenceTracker
├── client/                 # Angular frontend
│   └── src/
│       ├── app/            # Root module
│       ├── features/       # Feature modules
│       ├── core/           # Core services
│       ├── layout/         # Layout components
│       ├── shared/         # Shared utilities
│       └── types/          # TypeScript interfaces
└── docker-compose.yml      # SQL Server container
```

## API Endpoints

| Endpoint | Description |
|---|---|
| `POST /api/account/register` | Register a new user |
| `POST /api/account/login` | Login |
| `POST /api/account/refresh-token` | Refresh access token |
| `POST /api/account/logout` | Logout |
| `GET /api/members` | List members (paginated, filtered) |
| `GET /api/members/{id}` | Get member details |
| `POST /api/members/add-photo` | Upload a photo |
| `PUT /api/members/set-main-photo/{photoId}` | Set main profile photo |
| `DELETE /api/members/delete-photo/{photoId}` | Delete a photo |

## Real-Time Hubs

| Hub | Path | Purpose |
|---|---|---|
| MessageHub | `/hubs/messages` | Instant messaging, read receipts, message history |
| PresenceHub | `/hubs/presence` | Online/offline status, user presence tracking |
