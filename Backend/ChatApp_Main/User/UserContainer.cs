using ChatApp.Factory;
using ChatApp.Interface.User;
using System;
using System.Collections.Generic;

namespace ChatApp.User
{
    public class UserContainer
    {
        private IUserContainer userRepository;
        public UserContainer(IUserContainer userRepository = null)
        {
            if (userRepository == null) this.userRepository = UserFactory.CreateUserContainer();
            else userRepository = userRepository;
        }

        internal User CreateUser(UserDTO user, IUser userRepository = null)
        {
            return new User(user, userRepository);
        }

        internal User FindUserByUsername(string username)
        {
            UserDTO userDTO = userRepository.FindByUsername(username);

            return new User(userDTO);
        }

        public List<UserDTO> GetAllUsersAsDTO()
        {
            return userRepository.GetAllUsers();
        }
    }
}
