<p align="center">
  <img src="assets/logo_header.png" alt="Cardex Logo" width="800"/>
</p>

# Cardex

**Cardex** is a digital trading card game where players collect, trade, and race cars.  
Built as part of our COMP 4350 course project.  

## Project Overview
Cardex combines the excitement of collectible card games with the thrill of racing.  
Players can:
- Open randomized packs of car cards with unique stats and rarities  
- Trade cards with other players  
- Complete themed collections for rewards  
- Race cars based on their performance stats  
- Upgrade and customize their garage  

## Documentation

- Project Proposal: [Sprint 0 Proposal](./sprint0.md)
- Branching Strategies: [Branching Strategies](/docs/Branching-Strategies.md)
- Coding Conventions: [Coding Conventions](/docs/Coding-Conventions.md)

---

## Team Members - Group 7
- Alejandro Labra
- Ansh Nileshkumar Patel
- Vansh Chetankumar Shah
- Jotham Simiyon Stanlirajan
- Ian Spellman
- Subhash Yadav

---

## Tech Stack 
- **Frontend (Mobile):** Flutter  
- **Frontend (Web):** React  
- **Backend API:** ASP.NET Core 8 + Swagger/OpenAPI
- **Database:** PostgreSQL + Prisma, hosted on Supabase
- **DevOps:** Docker + GitHub Actions (CI/CD)  

---

## Repository Structure

    CarDexBackend/      # API (ASP.NET Core 8)
    CarDexFrontend/     # Web client (React)
    docs/               # Documentation


---

## Backend Setup (CarDexBackend)

### 🧩 Prerequisites
  - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (v8.0.414)
  - Git
  - Optional: Visual Studio 2022 or VS Code

Check installation:
  ```
  dotnet --version
  ```
  
  Clone and restore dependencies:

  ```
  git clone https://github.com/VSHAH1210/CarDex.git
  cd CarDexBackend
  dotnet restores
  ```

  Build all projects:
  ```
  dotnet build
  ```

### Running the Web API

From the project root:

```
dotnet run --project CarDexBackend/CarDexBackend.Api
```

Once running, visit:

  http://localhost:5083/swagger

Swagger will list all the controllers (Auth, Cards, Collections,   Packs, Trades, Users).

### Running Tests
From the project root,

Run all unit tests across the backend layer:
```
dotnet test
```

### Running Tests with Coverage (Coverlet)
From the project root,

Run tests and collect coverage:
```
dotnet test --collect:"XPlat Code Coverage"
```

Generate an HTML report:
```
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
```

Open the report in your browser:
```
start coveragereport/index.html
```

## AI Disclaimer

Portions of this project were developed with assistance from OpenAI’s ChatGPT.
Specifically, AI assistance was used to:

- Format XML documentation comments for controllers and DTOs.

- Provide mock service structure and test case suggestions for the unit tests.