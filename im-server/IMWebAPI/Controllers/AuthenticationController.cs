using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IMLibrary.Data;
using IMLibrary.Helpers;
using IMLibrary.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace IMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IM_API_Context _context;
        private readonly IEmailer _emailer;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationController(UserManager<ApplicationUser> userManager, IM_API_Context context, IEmailer emailer, IOptions<JwtSettings> jwtSettingsAccessor)
        {
            _userManager = userManager;
            _context = context;
            _emailer = emailer;
            _jwtSettings = jwtSettingsAccessor.Value;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(string email, string password, string job, string firstname, string lastname)
        {
            if (!ModelState.IsValid || email == null)
                return new BadRequestObjectResult(new { Message = "User Registration failed." });

            var appUser = new ApplicationUser() { UserName = email, Email = email, JobTitle = job, FirstName = firstname, LastName = lastname, Role = "Observer"};
            bool sendRegLink = false;
            if (string.IsNullOrEmpty(password))
            {
                // adding user from admin UI - create a temporary password and send a registration link
                sendRegLink = true;
                password = Guid.NewGuid().ToString();
            }

            // checks if email already exists
            var user = await _userManager.FindByNameAsync(email);
            if (user != null && user.Email == email)
            {
                return new BadRequestObjectResult(new { Message = "We already have an account under that email. Please try again." });
            }

            var result = await _userManager.CreateAsync(appUser, password);
            if (!result.Succeeded)
                return new BadRequestObjectResult(new { Message = "User Registration failed. Could not create new account." });

            result = await _userManager.AddToRoleAsync(appUser, "Observer");
            if (!result.Succeeded)
                return new BadRequestObjectResult(new { Message = "Role assignment failed." });


            if (sendRegLink)
            {
                _emailer.Send(email, "Create web portal account", "You have been added as a user on Canyons School District's IMseen web portal. Please follow the link below to activate your account.\n\n"
                     + $"{{webUrl}}/register?username={email}&token={password}");
            }

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

            var jwtGenerator = new JwtGenerator(_jwtSettings);

            // we only store username in claim for authentication
            // for security and to keep the jwt small, we fetch claims from the DB during authorization
            jwtGenerator.AddClaim(new Claim(ClaimTypes.Name, appUser.UserName));

            var accessToken = jwtGenerator.GetAccessToken();
            var refreshToken = jwtGenerator.GetRefreshToken();

            appUser.RefreshToken = refreshToken;
            if (appUser.Role == ApplicationUser.Observer)
            {
                appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(_jwtSettings.ObserverRefreshLifeInSecs);
            }
            else
            {
                appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshLifeInSecs);
            }

            var result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
                return new BadRequestObjectResult(new { Message = "Login failed. Token not generated." });

            return Ok(new { Token = accessToken, RefreshToken = refreshToken, Message = "You are logged in." });
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string username, string currentPassword, string password, string firstname, string lastname, string job)
        {
            if (!ModelState.IsValid || username == null || password == null || currentPassword == null)
                return new BadRequestObjectResult(new { Message = "Missing parameters" });

            var appUser = await _userManager.FindByNameAsync(username);
            if (appUser == null)
                return new BadRequestObjectResult(new { Message = "Login failed. Email not recognized." });

            var result = await _userManager.ChangePasswordAsync(appUser, currentPassword, password);
            if (!result.Succeeded)
                return new BadRequestObjectResult(new { Message = "Login failed. Password incorrect or does not meet requirements." });

            if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(job))
            {
                var user = _context.Users.Where(u => u.UserName == username).First();
                if (!string.IsNullOrEmpty(firstname))
                {
                    user.FirstName = firstname;
                }
                if (!string.IsNullOrEmpty(lastname))
                {
                    user.LastName = lastname;
                }
                if (!string.IsNullOrEmpty(job))
                {
                    user.JobTitle = job;
                }
                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = "Password changed successfully" });
        }

        [HttpGet]
        [Authorize(Policy = "WebAppUser")]
        [Route("User")]
        public async Task<IActionResult> GetUserInfo()
        {
            var username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new { User = user, Role = roles.First() });
        }

        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Logout Successful" });
        }

        /// <summary>
        /// Policies to access endpoints in this controller
        /// </summary>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("WebAppUser", policy => policy.RequireClaim("WebAppUser"));
        }
    }
}
