# Quiz App API

Backend provides functionalities such as managing and solving quizes and is built with REST API architecture. 
Project also implements user roles (user/admin) and JWT authentication (Httponly Cookies)

## Technologies

- **Framework:** .NET 8 (ASP.NET Core Web API)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Authentication:** JWT Token (Httponly Cookies)
- **Validation:** FluentValidation

## Local configuration

**1. Clone repository:**
\`\`\`bash
git clone https://github.com/TwojLogin/NazwaRepozytorium.git
cd RepositoryName
\`\`\`

**2. Configure credentials:**
Create your own .env file in main folder and add fields 
- CONN_STRING = connection string from postgres
- JWT_SECRET = create your own sercret (at least 32 marks)
- JWT_ISSUER = backend address
- JWT_AUDIENCE = frontend address

**3. Uruchom migracje bazy danych:**
Create database schema using migration. Type in terminal
\`\`\`bash
dotnet ef database update
\`\`\`

**4. Run app:**
\`\`\`bash
dotnet run
\`\`\`
App should start under address: `https://localhost:5216`.

## Project structure

- `/Controllers` - endpoints for api(like. `AuthController`, `PlayController`).
- `/Models` - Entities (User, Quiz, Question) and Dto's.
- `/Services` - Business logic (like checking answers, calculating score).
- `/Data` -  `ApplicationDbContext` config (Entity Framework).
- `/Extensions` - CORS, jwt config.

