using Identity_EntityFrameWork.Models;
using Identity_EntityFrameWork.Models.DTOs;
using Identity_EntityFrameWork.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity_EntityFrameWork.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDto)
        {

            var result = await _userService.RegisterService(registerDto);

            return Ok(result);

        }


        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginDTO loginDto)
        {

            var result = await _userService.LoginService(loginDto);

            return Ok(result);

        }


        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetAllUsers()
        {

            var result = await _userService.GetAllUsersService();

            return Ok(result);


        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(Guid id)
        {
            var result = await _userService.GetUserByIdService(id);

            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<AppUser>> GetUserByEmail(string email)
        {
            var result = await _userService.GetUserByEmailService(email);  

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<AppUser>> GetUserByName(string fullName)
        {
            var result = await _userService.GetUserByFullNameService(fullName);

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<AppUser>> GetUserByUserName(string userName)
        {
            var result = await _userService.GetUserByUserNameService(userName);

            return Ok(result);

        }

        [HttpGet]
        public async Task<ActionResult<AppUser>> GetUserStatus(string status)
        {
            var result = await _userService.GetUserByStatusService(status);

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<string>> UpdateUserEmail(string Id, string newEmail)
        {
            var result = await _userService.UpdateEmailService(Id, newEmail);

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<string>> UpdateUserPassword(string Id, string currentPassword, string newPassword)
        {
            var result = await _userService.UpdatePassword(Id, currentPassword, newPassword);

            if (!result)
            {
                return "Id not found or current password is incorrect";

            }

            return Ok("Password updated successfully");
        }

        [HttpPost]
        public async Task<ActionResult<string>> UpdateUserFullName(string Id, string newFullName)
        {
            var result = await _userService.UpdateFullNameService(Id, newFullName);

            if (result is null)
            {
                return "Id not found";
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> UpdateUserName(string Id, string newName)
        {
            var result = await _userService.UpdateUserNameService(Id, newName);
            if(result is null)
            {
                return "Id not found";
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<string>> UpdateUserAge(string Id, int newAge)
        {
            var result = await _userService.UpdateUserAgeService(Id, newAge);

            if(result is null)
            {
                return "Id not found";
            }

            return Ok(result);
        }





        [HttpPost]
        public async Task<ActionResult<string>> UpdateStatusService(string Id, string newStatus)
        {
            var result = await _userService.UpdateStatusService(Id, newStatus);

            if (result is null)
            {
                return "Id not found";
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUser(Guid id)
        {
            var result = await _userService.DeleteUser(id.ToString());

            if (!result)
            {
                return "Id not found";
            }

            return Ok("User deleted successfully");
        }
    }
}
