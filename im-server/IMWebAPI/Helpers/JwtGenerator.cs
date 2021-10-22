using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IMWebAPI.Helpers
{
    public class JwtGenerator
    {
        private readonly JwtHeader jwtHeader;
        private readonly IList<Claim> jwtClaims;
        private readonly DateTime jwtDate;
        private readonly int tokenLifetimeInSecs;
        //private readonly JwtBearerTokenSettings jwtBearerTokenSettings;

        public JwtGenerator()//JwtBearerTokenSettings jbts)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IMWEBAPI_SECRETKEY")); //jwtBearerTokenSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            this.jwtHeader = new JwtHeader(credentials);
            this.jwtClaims = new List<Claim>();
            this.jwtDate = DateTime.UtcNow;
            this.tokenLifetimeInSecs = 30; //jwtBearerTokenSettings.LifeInSecs;
        }

        public string GetAccessToken()
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                jwtHeader,
                new JwtPayload("IMWEBAPI", "IMWEBAPI", jwtClaims, jwtDate, jwtDate.AddSeconds(tokenLifetimeInSecs))
                //new JwtPayload(jwtBearerTokenSettings.Issuer, jwtBearerTokenSettings.Audience, jwtClaims, jwtDate, jwtDate.AddSeconds(tokenLifetimeInSecs))
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var parameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = "IMWEBAPI",//jwtBearerTokenSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = "IMWEBAPI",//jwtBearerTokenSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("IMWEBAPI_SECRETKEY")),//jwtBearerTokenSettings.Key)),
                ValidateLifetime = true,
                RequireAudience = true,
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ClockSkew = TimeSpan.Zero // USED FOR TESTING
            };

            var principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }




        public void AddClaim(Claim claim)
        {
            jwtClaims.Add(claim);
        }

    }
}
