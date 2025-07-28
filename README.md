# 🎵 MusicPlatform - C# Web - May 2025

This is a full-stack ASP.NET Core MVC web application for sharing and discovering music, built with a clean, decoupled architecture. The project is inspired by platforms like **SoundCloud**, this project is a comprehensive demonstration of building a feature-rich web application using modern .NET technologies. It was developed with a focus on clean architecture, maintainability, and production-ready patterns, including the Repository and Service patterns, role-based authorization, and dynamic frontend interactions. The platform allows users to register, upload their own tracks, create and manage playlists, and interact with other users' content through likes and comments.

### 🛠️ Technology Stack

This project is built on a robust and modern technology stack:

*   **Backend:** ASP.NET Core 8
*   **Database:** SQL Server with Entity Framework Core (Code-First)
*   **Authentication:** ASP.NET Core Identity with Roles
*   **File Storage:** **Cloudinary** for audio and image hosting
*   **Frontend:** Razor Views with Bootstrap
*   **Dynamic UX:** **Hotwired Turbo** for a seamless, single-page application feel without complex JavaScript frameworks.
*   **Architecture:**
    *   MVC (Model-View-Controller)
    *   Repository & Unit of Work Pattern
    *   Decoupled Service Layer
    *   Admin Area with Role-Based Authorization

## ✨ Features

*   **User Accounts:** Secure registration and login system.
*   **Admin Dashboard:** A dedicated area for administrators to manage users and site content like genres.
*   **Track Management:** Users can upload, edit, and soft-delete their own tracks.
*   **Playlist Management:** Users can create, edit, and delete their own public or private playlists.
*   **Dynamic Audio Player:** A global audio player that persists across page navigations thanks to Hotwired Turbo.
*   **Interactive Features:**
    *   Like/Unlike tracks with optimistic UI updates.
    *   Add/Remove tracks from playlists via an interactive modal.
    *   View, create and delete (own) comments on tracks.
*   **Discovery:** Paginated and searchable track listings.
*   **User Profiles:** View a user's uploaded tracks, public playlists, and favorited songs.

## ▶️ Getting Started

To get a local copy up and running, follow these simple steps.

### 📋 Prerequisites

*   .NET 8 SDK or later.
*   SQL Server Express or another edition of SQL Server.
*   An IDE like Visual Studio 2022 or VS Code.
*   A free **Cloudinary** account to get your API credentials.
  * - [Sign up for Cloudinary](https://cloudinary.com/users/register_free) to get your API credentials.

### 🚀 Project Setup

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/vass-k/MusicPlatform-ASP.NET-Core.git
    ```

2.  **Configure your secrets:**
    Open `MusicPlatform.Web/appsettings.Development.json` and fill in your configuration details. This file is NOT committed to source control and is safe for your secrets.

    *   **Database Connection String:**
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MusicPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        },
        ```

    *   **Cloudinary Credentials:**
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=.\\SQLEXPRESS;Database=MusicPlatform;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        },
        "CloudinarySettings": {
          "CloudName": "YOUR_CLOUD_NAME",
          "ApiKey": "YOUR_API_KEY",
          "ApiSecret": "YOUR_API_SECRET"
        },
        ```

    *   **Default Test and Admin User (Optional):**
        The application will seed a default test and admin user.
        ```json
        User: user@music.com 123
        Admin: admin@music.com 123
        ```

3.  **Apply Database Migrations:**
    *   Open the **Package Manager Console** in Visual Studio.
    *   Make sure the "Default project" is set to `MusicPlatform.Data`.
    *   Run the command:
        ```sh
        Update-Database
        ```

4.  **Run the Application:**
    *   Set `MusicPlatform.Web` as the startup project.
    *   Press `F5` or run `dotnet run` from the `MusicPlatform.Web` directory.

The application will launch, and the database will be automatically seeded with default genres and your admin user. You can now register a new "User" account or log in with the admin credentials.