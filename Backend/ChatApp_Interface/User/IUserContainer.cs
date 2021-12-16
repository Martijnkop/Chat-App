using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Interface.User
{
    public interface IUserContainer
    {
        public UserDTO FindByUsername(string username);
        public List<UserDTO> GetAllUsers();
    }
}
