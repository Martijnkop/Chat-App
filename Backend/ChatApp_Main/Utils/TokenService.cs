using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ChatApp.Utils
{
    public class TokenService
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
        }

        public bool ValidateToken(string t, out SecurityToken token)
        {
            string key = config["Jwt:Key"].ToString();
            string issuer = config["Jwt:Issuer"].ToString();
            string audience = config["Jwt:Audience"].ToString();

            var mySecret = Encoding.UTF8.GetBytes(key);
            var mySecurityKey = new SymmetricSecurityKey(mySecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(t, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = mySecurityKey,
                }, out SecurityToken vToken);

                token = vToken;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                token = null;
                return false;
            }

            return true;
        }
    }
}
