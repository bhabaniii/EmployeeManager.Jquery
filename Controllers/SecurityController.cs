using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EmployeeManager.Jquery.Models;
using EmployeeManager.Jquery.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManager.Jquery.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class SecurityController : ControllerBase
    { 
        private IConfiguration _config=null;
        private AppDbContext appDbContext=null;

        public SecurityController(IConfiguration config, AppDbContext _appDbContext)
        {
            this._config = config;
            this.appDbContext = _appDbContext;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody]Register userDetails)
        {
            //var usr = from u in appDbContext.users
            //          where u.UserName == userDetails.UserName
            //          select u;
             var usr = appDbContext.users.Where(u => u.UserName == userDetails.UserName);

            if (usr.Count() <= 0)
            {
                var user = new Users();
                user.UserName = userDetails.UserName;
                user.Password = userDetails.Password;
                user.Email = userDetails.Email;
                user.FullName = userDetails.FullName;
                user.BirthDate = userDetails.BirthDate;
                user.Role = "Manager";
                appDbContext.Add(user);
                appDbContext.SaveChanges();

                return Ok("User Created Successfully !");

            }

            else
            {

                return BadRequest("User Name already exists !");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public IActionResult SignIn([FromBody]SignIn loginDetails)
        {
            //var query = from u in appDbContext.users
            //            where u.UserName == loginDetails.UserName &&
            //            u.Password == loginDetails.Password
            //            select u;

             var query = appDbContext.users.Where(s => s.UserName == loginDetails.UserName && s.Password == loginDetails.Password);

            if (query.Count() > 0)
            {
                var tokenString = GenerateJWt(loginDetails.UserName);
                return Ok(new { token = tokenString });

            }

            else
            {

                return Unauthorized("User is not Authorized !");
            }

        }

        private string GenerateJWt(string userName)
        {
            //var usr = (from u in appDbContext.users
            //           where u.UserName == userName
            //           select u).SingleOrDefault();


             var usr = appDbContext.users.Where(u => u.UserName == userName).SingleOrDefault();

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, usr.UserName));
            claims.Add(new Claim(ClaimTypes.Role, usr.Role));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddHours(12),
                signingCredentials: credentials,
                claims: claims
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }

       

    }
}
