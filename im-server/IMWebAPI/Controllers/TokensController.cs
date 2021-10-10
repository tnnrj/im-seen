using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IMWebAPI.Data;
using IMWebAPI.Helpers;
using IMWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly JwtGenerator jwtGenerator;
        public TokensController(UserManager<ApplicationUser> userManager, JwtGenerator jwtGenerator)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(TokensController.userManager));
            this.jwtGenerator = this.jwtGenerator ?? throw new ArgumentNullException(nameof(TokensController.jwtGenerator));
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(string accessToken, string refreshToken)
        {
            if (accessToken is null || refreshToken is null)
            {
                return BadRequest("Invalid client request");
            }

            var principal = jwtGenerator.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = await userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

            foreach (Claim claim in principal.Claims)
            {
                jwtGenerator.AddClaim(claim);
            }

            var newAccessToken = jwtGenerator.GetAccessToken();
            var newRefreshToken = jwtGenerator.GetRefreshToken();

            user.RefreshToken = newRefreshToken;
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new BadRequestObjectResult(new { Message = "Token could not be refreshed. Please log in again." });

            return Ok(new { Token = newAccessToken, RefreshToken = refreshToken, Message = "Token Refresh Successful" });
        }



        [HttpPost, Authorize(Roles = "Administrator")]
        [Route("Revoke")]
        public async Task<IActionResult> Revoke(string username)
        {
            //var username = User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;
            var result = await userManager.UpdateAsync(user);

            return Ok( new { Message = "Refresh Token Successfully Revoked" });
        }
    }
}
