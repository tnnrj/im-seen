using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(string email, string password, string job, string firstname, string lastname)
        {
            if (!ModelState.IsValid || email == null || password == null)
                return new BadRequestObjectResult(new { Message = "User Registration failed." });

            var appUser = new ApplicationUser() { UserName = email, Email = email, JobTitle = job, FirstName = firstname, LastName = lastname };
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

            var appUser = await _userManager.FindByEmailAsync(username);
            if (appUser == null)
                return new BadRequestObjectResult(new { Message = "Login failed. Email not recognized." });

            var passResult = _userManager.PasswordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, password);
            if (passResult == PasswordVerificationResult.Failed)
                return new BadRequestObjectResult(new { Message = "Login failed. Incorrect password." });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.Name, appUser.Email),
                // Add more claims as necessary
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await Request.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Ok(new { Message = "You are logged in" });
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok(new { Message = "Logout Successful" });
        }
    }
}
