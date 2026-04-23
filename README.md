

# CyberQuiz 🎯

CyberQuiz is a web-based quiz application built for cybersecurity learning. It is designed to give users a simple and interactive way to register, sign in, practice their knowledge, and follow their progress over time. The solution is structured as a multi-project .NET 10 application with a Blazor UI, a Web API, a business logic layer, a data access layer, and shared contracts between projects.

## Project conclusion 📘

The purpose of CyberQuiz is to make cybersecurity learning more interactive, structured, and motivating. Instead of only reading theory, users actively test their knowledge through multiple-choice questions, get immediate feedback, and build progress step by step. By separating the solution into UI, API, BLL, DAL, and Shared projects, the application stays easier to understand, maintain, and extend.
<img width="2829" height="1522" alt="StartPage" src="https://github.com/user-attachments/assets/d0159748-8bf4-4910-87ab-7abc9df2b975" />
<img width="2323" height="1429" alt="QuizPage" src="https://github.com/user-attachments/assets/a15e6884-8489-4ed1-b04e-ec11a594ecbf" />
<img width="1536" height="1024" alt="CyberQuiz ER Diagram" src="https://github.com/user-attachments/assets/52823d15-09cf-4531-88f9-b66f9c0a008b" />


## What the project delivers 📦

CyberQuiz delivers a complete learning flow where users can:

- create an account and sign in
- browse categories and subcategories
- see which subcategories are locked or unlocked
- answer quiz questions and get direct feedback
- save each result to the database
- track overall progression
- use AI support after incorrect answers

The project is not only about showing questions on a page. It is built to demonstrate how a full-stack .NET application can combine usability, security, database persistence, and clear architecture in one solution.

## Main features ✨

- Interactive quiz experience in Blazor
- Category and subcategory progression
- Locked and unlocked content flow based on user progress
- 80% completion rule to unlock the next subcategory
- Instant answer validation
- Score and completion tracking
- AI help for incorrect answers
- ASP.NET Core Identity authentication in the UI project
- Swagger for API testing
- Automatic database migration on startup for the UI identity database
- Automatic migration and quiz data seeding in development for the API database

## How to play 🎮

1. Start both the API and UI projects.
2. Open the Blazor UI in your browser.
3. Register or sign in.
4. Go to the category overview and choose a topic.
5. Review the available subcategories and see which ones are locked or unlocked.
6. Start a quiz in any unlocked subcategory.
7. Select one answer for each question and submit it.
8. Get direct feedback showing whether your answer was correct or incorrect.
9. Continue through the quiz until all questions in the subcategory are completed.
10. Reach at least 80% correct answers to unlock the next subcategory.
11. Return to the overview and continue building your progress.



## Solution structure 🏗️

The solution contains five projects:

- **CyberQuiz.UI**  
  Blazor UI on .NET 10. Handles authentication, pages, navigation, and API communication.

- **CyberQuizAPI**  
  ASP.NET Core Web API. Exposes quiz endpoints, configures CORS, applies migrations, and seeds quiz data in development.

- **CyberQuiz.BLL**  
  Business logic layer. Handles progression rules, scoring, answer validation, and quiz flow.

- **CyberQuiz.DAL**  
  Data access layer. Contains Entity Framework Core context, repositories, migrations, entities, and seed logic.

- **CyberQuiz.Shared**  
  Shared DTOs and contracts used between projects.

## Functions in the system ⚙️

### UI functions
- User sign in and registration
- Category and question pages
- Submit-answer workflow
- Quiz result display
- API integration from Blazor
- Profile and progression overview

### API functions
- Get quiz categories for a user
- Get subcategories for a selected category
- Get questions for a selected subcategory
- Submit answers and return correctness data
- Get user progress
- Provide Swagger documentation for testing

### Business logic functions
- Calculate progress
- Lock and unlock subcategories
- Validate answers
- Save user results
- Return result DTOs to the API
- Apply the 80% progression rule

### Data functions
- Store categories, subcategories, questions, answers, and user results
- Apply Entity Framework Core migrations
- Seed quiz data in development

## Technologies used 💻

- .NET 10
- C#
- Blazor
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server / LocalDB
- ASP.NET Core Identity
- Swagger / Swashbuckle
- Dependency Injection
- Layered architecture
- REST API



## Default local ports 🌐

### UI
- HTTP: `http://localhost:5063`
- HTTPS: `https://localhost:7255`

### API
- HTTP: `http://localhost:5275`
- HTTPS: `https://localhost:7050`
- Swagger: `https://localhost:7050/swagger`

## Setup 🚀

### Prerequisites

Install the following before running the solution:

- Visual Studio 2026 or later with .NET workload
- .NET 10 SDK
- SQL Server LocalDB
- Optional: `dotnet-ef` global tool for manual migration commands

### Restore packages

```powershell
dotnet restore
```

## Database setup 🗄️

This solution uses two database responsibilities:

- **CyberQuiz.UI** uses `ApplicationDbContext` for ASP.NET Core Identity
- **CyberQuizAPI`/`CyberQuiz.DAL** use `CyberQuizDbContext` for quiz data

We use two database contexts because the project is built as a Blazor Web App with a separate API and quiz domain. The UI project needs its own Identity database context for login, registration, and account management, while the API and DAL use a separate quiz database context for categories, questions, answers, and user results. This separation keeps authentication concerns apart from quiz data and makes the solution easier to maintain.

### Update the UI Identity database

The UI project applies migrations on startup, but you can also update it manually:

```powershell
cd C:\Users\supap\Desktop\Github_Projects\CyberQuiz\CyberQuiz.UI
dotnet ef database update
```

Or from the solution root:

```powershell
dotnet ef database update --project .\CyberQuiz.UI\CyberQuiz.UI.csproj --startup-project .\CyberQuiz.UI\CyberQuiz.UI.csproj
```

### Update the API quiz database

From the solution root:

```powershell
dotnet ef database update --project .\CyberQuiz.DAL\CyberQuiz.DAL.csproj --startup-project .\CyberQuizAPI\CyberQuizAPI.csproj
```

> Note: The API applies migrations and seeds quiz data automatically on startup in development.

### Trust HTTPS development certificate

If HTTPS fails locally:

```powershell
dotnet dev-certs https --trust
```

## How to choose multiple startup projects in Visual Studio ▶️

1. Open the solution in Visual Studio.
2. Right-click the solution in Solution Explorer.
3. Select **Configure Startup Projects**.
4. Choose **Multiple startup projects**.
5. Set these projects to **Start**:
   - `CyberQuizAPI`
   - `CyberQuiz.UI`
6. Start debugging.

Recommended startup order:
1. `CyberQuizAPI`
2. `CyberQuiz.UI`

## How to run from command line 🖥️

Open two terminals.

### Terminal 1 - API

```powershell
cd C:\Users\supap\Desktop\Github_Projects\CyberQuiz\CyberQuizAPI
dotnet run
```

### Terminal 2 - UI

```powershell
cd C:\Users\supap\Desktop\Github_Projects\CyberQuiz\CyberQuiz.UI
dotnet run
```


## Architecture notes 🧩

The solution follows a layered architecture:

- UI calls API
- API calls BLL services
- BLL uses DAL repositories and unit of work
- DAL accesses SQL Server through Entity Framework Core
- Shared contains DTOs exchanged between projects

This separation helps with maintenance, readability, and future testing.

The **Unit of Work** pattern is used in the DAL to group related repository operations into one coordinated database action. This helps the business layer work with multiple repositories in a cleaner way and keeps data changes more consistent by saving them through a single entry point.

## Troubleshooting 🛠️

### Unable to connect to web server `https`

Possible causes:
- HTTPS development certificate is not trusted
- Database migrations have not been applied
- The application crashed during startup
- The configured port is already in use

Suggested checks:

```powershell
dotnet dev-certs https --trust
netstat -ano | findstr 7255
netstat -ano | findstr 7050
```

### Invalid column name `DisplayName` >> Update database

This means the UI identity database schema is behind the current model. Apply the UI migrations:

```powershell
cd C:\Users\supap\Desktop\Github_Projects\CyberQuiz\CyberQuiz.UI
dotnet ef database update
```

## Suggested future improvements 🔮

- Add automated unit and integration tests
- Add role-based authorization for admin content management
- Add leaderboard or analytics features
- Improve deployment configuration
- Improve error handling and logging consistency


