using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CMApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CMApi.Controllers
{
    
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Admin") ]
        [Route("/test")]
        public string Test() {
            return "Woo";
        }

        [Route("/Authenticate")]
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(string username, string password)
        {
            if(await IsValidUser(username, password))
            {
                var token = await GenerateToken(username);
                var ob = new ObjectResult(token);
                return ob;
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidUser(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(username);
                return await _userManager.CheckPasswordAsync(user, password);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            double validForDays;
            string jwtSecret;

            try
            {
                validForDays = double.Parse(_configuration["Jwt:ValidForDays"]);
                jwtSecret = _configuration["Jwt:SecretKey"];
            }
            catch
            {

                throw new Exception("Error loading appsettings info for AuthController");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(validForDays)).ToUnixTimeSeconds().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                        new JwtHeader(
                            new SigningCredentials(
                                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                                SecurityAlgorithms.HmacSha256)),
                        new JwtPayload(claims));

            var output = new
            {
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = username,
                Roles = roles.Select(r => r.Name).ToList()
            };

            return output;
        }
    }
}
