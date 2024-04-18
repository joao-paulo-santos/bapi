using Microsoft.AspNetCore.Mvc;
using System;
using Core.Entities;
using Infrastructure.Persistence;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace bapi.Controllers
{
    [ApiController]
    [Route("API/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IConfiguration _config;

        public AuthenticationController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet(Name = "getUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return _context.Users.ToList();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> Post([FromBody] loginex loginRequest)
        {
            User? user = _context.Users.Where(x => x.Username == loginRequest.username).FirstOrDefault();
            if (user == null)
            {
                return Unauthorized("Username not found.");
            }
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.password));
            }
            string stringHash = Convert.ToHexString(hash);
            if(stringHash != user.Password.ToUpper())
            {
                return Unauthorized("Invalid Credentials");
            }
            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(token);
        }
    }

    public class loginex
    {
        public string username {  get; set; }
        public string password { get; set; }  
    }
}
