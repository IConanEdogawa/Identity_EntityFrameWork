using Identity_EntityFrameWork.Models;
using Identity_EntityFrameWork.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Identity_EntityFrameWork.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JWTTokenService _jWTTokenService;

   
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> RegisterService(RegisterDTO registerDto)
        {


            var user = new AppUser
            {
                FullName = registerDto.FullName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                Age = registerDto.Age,
                Status = registerDto.Status
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(error => $"{error.Code}: {error.Description}"));
                    throw new ApplicationException($"Unable to create user. Errors: {errors}");
                }
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database-related exceptions, such as unique constraint violations
                // Log the exception and return an appropriate response or rethrow the exception as needed
                // Example:
                // Log.Error("Database error occurred while creating user", ex);
                throw;
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                // Log the exception and return an appropriate response or rethrow the exception as needed
                // Example:
                // Log.Error("An unexpected error occurred while creating user", ex);
                throw;
            }


            foreach (var role in registerDto.Roles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return user;
        }


        // Get methods started

        public async Task<List<AppUser>> GetAllUsersService()
        {
            var result = await _userManager.Users.ToListAsync();

            return result;
        }

        public async Task<ActionResult<JWTToken>> LoginService(LoginDTO loginDto)
        {
            JWTToken token = null;
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                // User's email is not found
                return null;
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!isPasswordCorrect)
            {
                // Password is not correct
                return null;
            }

            // Authentication successful
            var jwtService = new JWTTokenService("conanedogawa", "detective");
            token = jwtService.GenerateToken(user.Id, user.UserName);


            return token;
        }

        public async Task<AppUser> GetUserByIdService(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                return null;
            }
            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user is null)
            {
                return null;
            }

            return user;
        }


        public async Task<AppUser> GetUserByEmailService(string email)
        {
            // Check if email is null or empty
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            // Check if email contains '@'
            if (!email.Contains('@'))
            {
                return null;
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);

            // Check if user not found
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<AppUser> GetUserByUserNameService(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }

            var result = await _userManager.FindByNameAsync(userName);

            if(result == null)
            {
                return null;
            }
            return result;
        }


        public async Task<AppUser> GetUserByFullNameService(string fullName)
        {
            if(string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            var result = await _userManager.Users.FirstOrDefaultAsync(u => u.FullName == fullName);

            if(result == null)
            {
                return null;
            }

            return result;
        }


        public async Task<List<AppUser>> GetUserByStatusService(string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return null;
            }

            // Check if status is valid ("Active" or "Inactive")
            if (status != "Active" && status != "Inactive")
            {
                return null;
            }

            var users = await _userManager.Users.Where(u => u.Status == status).ToListAsync();

            if (users == null || users.Count == 0)
            {
                return null;
            }

            return users;
        }




        // Update methods started

        public async Task<AppUser> UpdateFullNameService(string userId, string newFullName)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if (userToUpdate == null)
            {
                return null;
            }

            userToUpdate.FullName = newFullName;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);

            if (updateResult.Succeeded)
            {
                return userToUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<AppUser> UpdateUserNameService(string userId, string newUserName)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if (userToUpdate == null)
            {
                return null;
            }

            userToUpdate.UserName = newUserName;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);

            if (updateResult.Succeeded)
            {
                return userToUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<AppUser> UpdateUserAgeService(string userId, int newAge)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if (userToUpdate == null)
            {
                return null;
            }

            // Assuming Age is a property of AppUser
            userToUpdate.Age = newAge;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);

            if (updateResult.Succeeded)
            {
                return userToUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<AppUser> UpdateStatusService(string userId, string newStatus)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if (userToUpdate == null)
            {
                return null;
            }

            // Assuming Status is a property of AppUser
            userToUpdate.Status = newStatus;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);

            if (updateResult.Succeeded)
            {
                return userToUpdate;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> UpdateEmailService(string userId, string newEmail)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if(userToUpdate == null)
            {
                return "Email isn`t correct or found";
            }

            userToUpdate.Email = newEmail;

            var updateResult = await _userManager.UpdateAsync(userToUpdate);

            if(updateResult.Succeeded)
            {
                return "Email changed successfully";
            }
            else
            {
                return "Failed to change email";
            }


        }


        public async Task<bool> UpdatePassword(string userId, string currentPassword, string newPassword)
        {
            var userToUpdate = await _userManager.FindByIdAsync(userId);

            if (userToUpdate == null)
            {
                // User not found
                return false;
            }

            // Check if the current password provided is valid
            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(userToUpdate, currentPassword);

            if (!isCurrentPasswordValid)
            {
                // Current password is not correct
                return false;
            }

            // Change user's password
            var changePasswordResult = await _userManager.ChangePasswordAsync(userToUpdate, currentPassword, newPassword);

            if (changePasswordResult.Succeeded)
            {
                // Password changed successfully
                return true;
            }
            else
            {
                // Failed to change password
                // You can handle errors or log them as needed
                return false;
            }

            // Delete methods started



        }

        public async Task<bool> DeleteUser(string userId)
        {
            var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(userId));

            if(result.Succeeded)
            {
                return true;
            }

            return false;
        }




        // All comments wroten by me
        // because I learn this from google 
    }
}
