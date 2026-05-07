using WebApplication1.Models.Dtos.auth;

namespace WebApplication1.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterDto dto);
        Task<(bool Success, string Message, List<UserDto> Users)> GetAllUsersAsync();

        Task<(bool Success, string Message, string? Token)> LoginAsync(LoginDto dto);

        Task<(bool Success, string Message, UserDto? dto)> GetUserByIdAsync(int id);


    }
}
