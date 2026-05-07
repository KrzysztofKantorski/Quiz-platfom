using DotNetEnv;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Extensions;
using WebApplication1.Middlewares;
using WebApplication1.Models.Dtos.admin;
using WebApplication1.Models.Dtos.auth;
using WebApplication1.Models.Dtos.play.Request;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;
using WebApplication1.Validators;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);

var connString = Environment.GetEnvironmentVariable("CONN_STRING");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connString));

//Validators
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<IValidator<CreateQuizDto>, QuizDtoValidator>();
builder.Services.AddScoped<IValidator<CreateQuestionDto>, CreateQuestionDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateQuestionDto>, UpdateQuestionDtoValidator>();
builder.Services.AddScoped<IValidator<SubmitQuizDto>, SubmitQuizDtoValidator>();
builder.Services.AddScoped<IValidator<AnswerSubmissionDto>, AnswerSubmissionDtoValidator>();


//Global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//jwt
builder.Services.JWTAuthentication(builder.Configuration);

//CORS
builder.Services.AddCustomCors(builder.Configuration);


builder.Services.AddControllers();

//Controllers
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminQuizService, AdminQuizService>();
builder.Services.AddScoped<IAdminQuestionService, AdminQuestionService>();
builder.Services.AddScoped<IPlayQuizService, PlayQuizService>();
builder.Services.AddScoped<IQuizHistoryService, QuizHistoryService>();


var app = builder.Build();
app.UseCors();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
