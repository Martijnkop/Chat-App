using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Interface.User
{
    public interface IUser
    {
        bool AddUserToDatabase(UserDTO user);
        void SetConnectionID(string username, string connectionID);
    }
}
