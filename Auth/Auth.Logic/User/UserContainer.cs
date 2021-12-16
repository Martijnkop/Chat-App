using Auth.Factory;
using Auth.Interface.Token;
using Auth.Interface.User;
using Auth.Logic.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Logic.User
{
    public class UserContainer
    {
        private IUserRepository userRepository;

        public UserContainer(IUserRepository repository = null)
        {
            if (repository == null) this.userRepository = UserFactory.CreateUserRepository();
            else this.userRepository = repository;
        }

        public User CreateUser(AccountRegister account)
        {
            User user = new User(account);
            return user;
        }

        public User CreateUser(AccountLogin login)
        {
            User user = new User(login);
            return user;
        }

        public UserDTO GetAsDTO(AccountLogin login)
        {
            string salt = Salt.CreateSalt(login.Email);
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(login.Email + login.Password));
            string hashedPass = Salt.ByteArrayToString(hashedDataBytes);

            UserDTO userDTO = userRepository.GetUser(new AccountLogin { Email = login.Email, Password = hashedPass });

            return userDTO;
        }


        public User Get(AccountLogin login, IUser iUser = null)
        {
            return FromDTO(GetAsDTO(login), iUser);
        }

        private User FromDTO(UserDTO dto, IUser iUser = null)
        {
            return new User(email: dto.Email, username: dto.UserName, password: dto.Password, repository: iUser);
        }

        public bool LogoutUser(char[] refreshToken)
        {
            // Check if refresh token exists

            if (refreshToken == null || refreshToken.Length != 255) return false;

            // If the refreshtoken is not in the database, you can assume the user is logged out

            if (userRepository.RefreshTokenExists(refreshToken) == false) return true;

            // Logout refreshtoken

            return userRepository.LogoutUserByRefreshToken(refreshToken);
        }

        public UserDTO GetUserByRefreshToken(char[] refreshToken)
        {
            if (refreshToken == null || refreshToken.Length != 255) return null;

            if (userRepository.RefreshTokenExists(refreshToken) == false) return null;

            return userRepository.GetUserByRefreshToken(refreshToken);
        }
    }
}
