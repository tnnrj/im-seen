using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IMWebAPI.Helpers;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace IMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly JwtBearerTokenSettings jwtBearerTokenSettings;

        public AuthenticationController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(string username, string password, string job, string firstname, string lastname)
        {
            if (!ModelState.IsValid || username == null || password == null)
                return new BadRequestObjectResult(new { Message = "User Registration failed." });

            var appUser = new ApplicationUser() { UserName = username, Email = username, JobTitle = job, FirstName = firstname, LastName = lastname };
            var regResult = await _userManager.CreateAsync(appUser, password);
            if (!regResult.Succeeded)
                return new BadRequestObjectResult(new { Message = "User Registration failed. Could not create new account." });

            return Ok(new { Message = "User Registration Successful" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (!ModelState.IsValid || username == null || password == null)
                return new BadRequestObjectResult(new { Message = "Login failed." });

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return new BadRequestObjectResult(new { Message = "Login failed. Email not recognized." });

            var passResult = _userManager.PasswordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, password);
            if (passResult == PasswordVerificationResult.Failed)
                return new BadRequestObjectResult(new { Message = "Login failed. Incorrect password." });

            var jwtGenerator = new JwtGenerator();

            jwtGenerator.AddClaim(new Claim(ClaimTypes.Email, appUser.Email));
            jwtGenerator.AddClaim(new Claim(ClaimTypes.Name, appUser.UserName));
            // Add more claims as necessary (ROLES)


            return Ok(new { Token = jwtGenerator.GetToken(), Message = "You are logged in" });
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            return Ok(new { Message = "Logout Successful" });
        }
    }
}
