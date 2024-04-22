using Identity_EntityFrameWork.Models.DTOs;
using Identity_EntityFrameWork.Models;

namespace Identity_EntityFrameWork.Services.Abstractions
{
    public interface IUserService
    {
        Task<AppUser> RegisterService(RegisterDTO registerDto);
        Task<List<AppUser>> GetAllUsersService();
        Task<JWTToken> LoginService(LoginDTO loginDto);
        Task<AppUser> GetUserByIdService(Guid Id);
        Task<AppUser> GetUserByEmailService(string email);
        Task<AppUser> GetUserByUserNameService(string userName);
        Task<AppUser> GetUserByFullNameService(string fullName);
        Task<List<AppUser>> GetUserByStatusService(string status);
        Task<AppUser> UpdateFullNameService(string userId, string newFullName);
        Task<AppUser> UpdateUserNameService(string userId, string newUserName);
        Task<AppUser> UpdateUserAgeService(string userId, int newAge);
        Task<AppUser> UpdateStatusService(string userId, string newStatus);
        Task<string> UpdateEmailService(string userId, string newEmail);
        Task<bool> UpdatePassword(string userId, string currentPassword, string newPassword);
        Task<bool> DeleteUser(string userId);
    }
}
