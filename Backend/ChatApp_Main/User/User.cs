using ChatApp.Factory;
using ChatApp.Interface.User;
using System;

namespace ChatApp.User
{
    public class User
    {
        private IUser userRepository;
        public string Username { get; private set; }
        public string ConnectionID { get; private set; }

        public User(IUser repository = null)
        {
            if (repository == null) this.userRepository = UserFactory.CreateUser();
        }

        public User(UserDTO user, IUser repository = null)
        {
            if (repository == null) this.userRepository = UserFactory.CreateUser();

            this.Username = user.Username;
            this.ConnectionID = user.ConnectionID;
        }

        public void SetConnectionID(string id)
        {
            this.ConnectionID = id;

            userRepository.SetConnectionID(Username, ConnectionID);
        }

        internal bool AddToDatabase()
        {
            UserDTO userDTO = new UserDTO();
            userDTO.Username = this.Username;


            return userRepository.AddUserToDatabase(userDTO);
        }
    }
}
