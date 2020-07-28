using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using CMApi.Data;
using CMApi.Library;
using CMApi.Library.DataAccess;
using CMApi.Library.Models;
using CMApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace CMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserData _userData;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext context, IUserData userData, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userData = userData;
            _userManager = userManager;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userData.GetUserById(userId);
            return user;
        }


        [HttpGet]
        [Route("/api/user/admin/getusers")]
        public List<ApplicationUserModel> GetAllUsers(string type)
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

                if(u.Roles.Values.ToList().IndexOf(type) != -1)
                {
                    output.Add(u);
                }
            }

            return output;
        }

        [HttpGet]
        [Route("/api/user/admin/findbyemail")]
        public async Task<ApplicationUserModel> SearchByEmail(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            if(user == null)
            {
                return null;
            }

            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            ApplicationUserModel u = new ApplicationUserModel
            {
                Id = user.Id,
                Email = user.Email
            };

            u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);


            return u;

        }

        [HttpGet]
        [Route("/api/user/admin/roles")]
        public Dictionary<string, string> GetAllRoles()
        {
            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            return roles;
        }

        [HttpPost]
        [Route("/api/user/admin/roles/add")]
        public async Task AddARole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);

            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }

        [HttpPost]
        [Route("/api/user/admin/roles/remove")]
        public async Task RemoveARole(UserRolePairModel pairing)
        {
            var user = await _userManager.FindByIdAsync(pairing.UserId);

            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }

    }
}
