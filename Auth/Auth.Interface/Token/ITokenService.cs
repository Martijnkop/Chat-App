using Auth.Interface.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;


namespace Auth.Interface.Token
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, UserDTO user, string audience = null, DateTime? date = null);
        bool ValidateToken(string key, string issuer, string audience, string token);
    }
}
