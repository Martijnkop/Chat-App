using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Interface.User
{
    public interface IUser
    {
        bool UserExists(string token, string type);
        bool AddUser(UserDTO userDTO, char[] refreshToken);
        bool UpdateRefreshToken(string email, string pass, char[] refreshToken);
    }
}
