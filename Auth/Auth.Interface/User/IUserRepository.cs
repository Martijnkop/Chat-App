using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interface.User
{
    public interface IUserRepository
    {
        UserDTO GetUser(AccountLogin login);
        bool RefreshTokenExists(char[] refreshToken);
        bool LogoutUserByRefreshToken(char[] refreshToken);
        UserDTO GetUserByRefreshToken(char[] refreshToken);
    }
}
