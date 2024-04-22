using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using bapi.Dtos;
using Core.Interfaces;
using bapi.Mappers;
using System.Security.Claims;
using System;

namespace bapi.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationController(ITokenService tokenService,IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        
        [Authorize(Roles = "Admin")]
        [Route("getUserByUsername")]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUserByUsername(string username)
        {
            User? user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return NotFound("Username not found.");
            return Ok(UserMapper.UserToDto(user));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("getUsers")]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsers()
        {
            var list = await _userService.GetListOfUsersAsync();
            List<UserDto> users = list.Select(x => UserMapper.UserToDto(x)).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("getUsersPaged")]
        public async Task<ActionResult<IReadOnlyList<UserDto>>> GetUsersPaged(int pageIndex = 0, int pageSize = 10)
        {
            var list = await _userService.GetPagedListOfUsersAsync(pageIndex, pageSize);
            List<UserDto> users = list.Select(x => UserMapper.UserToDto(x)).ToList();
            return Ok(list);
        }

        [Authorize]
        [Route("getClaim")]
        [HttpGet]
        public ActionResult<string> getClaim()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var usernameClaim = user.FindFirstValue(ClaimTypes.Name);
            var roleClaim = user.FindFirstValue(ClaimTypes.Role);

            return Ok(new { Username = usernameClaim, Role = roleClaim });
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<UserClaimsDto>> Login([FromBody] LoginDto loginRequest)
        {
            User? user = await _userService.GetUserByUsernameAsync(loginRequest.Username);
            if (user == null) return Unauthorized("Username not found.");

            if(!_userService.VerifyPassword(user,loginRequest.Password))
            {
                return Unauthorized("Invalid Credentials");
            }

            var token = _tokenService.CreateToken(user);

            return UserMapper.UserToUserClaimsDto(user, token);
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<UserClaimsDto>> Register([FromBody] RegisterDto registerRequest)
        {
            User? user = await _userService.GetUserByUsernameAsync(registerRequest.Username);
            if (user != null) return Conflict("Username already in use.");
            
           user = await _userService.RegisterUserAsync(registerRequest.Username, registerRequest.Password);
            if (user == null) return Problem();
            

            var token = _tokenService.CreateToken(user);

            return UserMapper.UserToUserClaimsDto(user, token);
        }

        [Authorize(Roles = "Admin")]
        [Route("changeRole")]
        [HttpPatch]
        public async Task<ActionResult<UserDto>> ChangeRole([FromBody] ChangeRoleDto changeRoleDto)
        {
            User? user = await _userService.GetUserByUsernameAsync(changeRoleDto.Username);
            if (user == null) return NotFound("User not found");
            
 
            bool result = Enum.TryParse(changeRoleDto.NewRole, true, out Role newRole) && Enum.IsDefined(typeof(Role), newRole);
            if (!result) return BadRequest("Invalid Role");

            user = await _userService.ChangeUserRoleAsync(user, newRole);
            if (user == null) return Problem("Internal Error, Could not update the user");

            return UserMapper.UserToDto(user);
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteUser")]
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteUser([FromBody] string username)
        {
            User? user = await _userService.GetUserByUsernameAsync(username);
            if (user == null) return NotFound("User not found");


            bool result = await _userService.DeleteUserAsync(user);
            if (!result) return Problem("Internal Error, Could not delete the user");

            return Ok(true);
        }
    }
}
