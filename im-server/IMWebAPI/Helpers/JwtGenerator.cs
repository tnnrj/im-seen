using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
            this.tokenLifetimeInSecs = 7200; //jwtBearerTokenSettings.LifeInSecs;
        }

        public string GetToken()
        {
            JwtSecurityToken jwt = new JwtSecurityToken(
                jwtHeader,
                new JwtPayload("IMWEBAPI", "IMWEBAPI", jwtClaims, jwtDate, jwtDate.AddSeconds(tokenLifetimeInSecs))
                //new JwtPayload(jwtBearerTokenSettings.Issuer, jwtBearerTokenSettings.Audience, jwtClaims, jwtDate, jwtDate.AddSeconds(tokenLifetimeInSecs))
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public void AddClaim(Claim claim)
        {
            jwtClaims.Add(claim);
        }

    }
}
