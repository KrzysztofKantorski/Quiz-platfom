# Quiz App API

The backend provides functionalities for managing and solving quizzes, built on a REST API architecture. The system supports user roles (Admin/User) and secure JWT authentication using HttpOnly cookies.

## Technologies

* Framework: .NET 8 (ASP.NET Core Web API)
* Database: PostgreSQL
* ORM: Entity Framework Core
* Authentication: JWT Token (HttpOnly Cookies)
* Validation: FluentValidation

## Local configuration

1. Clone the repository:
```bash
git clone https://github.com/YourUsername/RepositoryName.git
```

3. Configure credentials:
Create an .env file in the root folder of the project and add the following fields:
- CONN_STRING = your PostgreSQL connection string
- JWT_SECRET = your secret key (at least 32 characters)
- JWT_ISSUER = backend address (e.g., https://localhost:5216)
- JWT_AUDIENCE = frontend address (e.g., http://localhost:5173)

3. Run database migrations:
Type the following command in the terminal to create database tables:
```bash
dotnet ef database update
```
5. Run the app:
```bash
dotnet run
```
The app starts by default at: https://localhost:5216

## Project structure

* /Controllers - API endpoints (e.g., Auth, Play, Admin)
* /Models - Database entities and DTOs
* /Services - Business logic (calculating scores, checking answers)
* /Data - Database configuration (DbContext)
* /Extensions - CORS and JWT configuration
* /Validators - Validate incomming data
* /Middlewares - Catch errors
* /Migrations - Database migrations created with Entity Framework
  
## Quiz Flow

When a quiz starts, the client receives a single, full JSON response from the server. It contains the entire quiz structure: all questions along with available answers. This allows the client application to smoothly switch between questions without constantly making requests to the server. After finishing the quiz, the client sends a single payload with all selected answers back to the server for validation and score calculation.

## API Reference

### Authentication (Auth)
- POST /api/auth/register - Register a new user
- POST /api/auth/login - Login (sets HttpOnly cookie)

### Admin Panel (Admin role required)
- GET /api/admin/quizzes - Get a list of all quizzes
- POST /api/admin/quizzes - Create a new quiz with questions
- PUT /api/admin/quizzes/{id} - Update an existing quiz
- DELETE /api/admin/quizzes/{id} - Delete a quiz

### Play (User role required)
- GET /api/play/quizzes - Get a list of available quizzes to solve
- GET /api/play/quizzes/{id}/play - Fetch full JSON with questions (Start quiz)
- POST /api/play/quizzes/{id}/submit - Submit answers and calculate score
- GET /api/play/history - Get logged user's quiz history
