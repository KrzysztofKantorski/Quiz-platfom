using BCrypt.Net;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Models.Dtos.auth;
using WebApplication1.Models.Entities;
using WebApplication1.Services.Interfaces;
namespace WebApplication1.Services
{
    public class AuthService: IAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        public AuthService(ApplicationDbContext dbContext, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator) {
            _dbContext = dbContext;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto)
        {
            //Validation of dto
            await _registerValidator.ValidateAndThrowAsync(dto);

            //Check if email is in use
            var emailInUse = await _dbContext.Users.AnyAsync(u => u.Email == dto.Email);
           
            if (emailInUse)
            {
                return (false, "Email address is alerdy in use");
            }

            //Password hash
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            //Map to entity
            var newUser = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = "User"
            };

            //Save user to database
            await _dbContext.AddAsync(newUser);

            await _dbContext.SaveChangesAsync();
            return (true, "Registration went successfully");

        }


        public async Task<(bool Success, string Message, List<UserDto> Users)> GetAllUsersAsync()
        {
            var getUsers = await _dbContext.Users.Select(
                u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role,
                }
                ).ToListAsync();

            if (getUsers.Count == 0)
            {
                return (true, "No users found", getUsers);
            }
            return (true, "Found users: ", getUsers);
        }




        public async Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto dto) {
            //Validation of dto
            await _loginValidator.ValidateAndThrowAsync(dto);

            //Find user
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == dto.Email); 

            if(user is null)
            {
                return (false, "User not found", null);
            }

            //Check password
            var validPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!validPassword)
            {
                return (false, "Incorrect credentials", null);
            }

            //Get token settings
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")!;
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")!;
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")!;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Claims
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                 new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            //Create token
            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );
            
            //Toekn serialization
            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);
            return (true, "Login Successfull", tokenHandler);
        }
        

        public async Task<(bool Success, string Message, UserDto? dto)> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role,
                })
                .FirstOrDefaultAsync();
            if(user is null)
            {
                return (false, "Couldnt find user ", null);
            }

            return (true, "User profile was found", user);
        }
    }
}
