using Auth.Factory;
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
    public class User
    {
        private IUser userRepository;

        private string Email { get; set; }
        private string Username { get; set; }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Password { get; set; }
        private char[] RefreshToken { get; set; }

        public User(AccountRegister account, IUser repository = null)
        {
            if (repository == null) this.userRepository = UserFactory.CreateUser();
            else this.userRepository = repository;

            this.Email = account.Email;
            this.Username = account.Username;
            this.FirstName = account.FirstName;
            this.LastName = account.LastName;
            this.Password = account.Password;
        }

        public User(AccountLogin login, IUser repository = null)
        {
            if (repository == null) this.userRepository= UserFactory.CreateUser();
            else this.userRepository = repository;

            this.Email = login.Email;
            this.Password = login.Password;
        }

        public User(string email = null, string username = null, string firstname = null, string lastname = null, string password = null, IUser repository = null)
        {
            if (repository == null) this.userRepository = UserFactory.CreateUser();
            else this.userRepository = repository;

            this.Email = email;
            this.Username = username;
            this.FirstName = firstname;
            this.LastName = lastname;
            this.Password = password;
        }

        public bool AddToDatabase()
        {
            if (string.IsNullOrEmpty(this.Email) ||
                string.IsNullOrEmpty(this.Username) ||
                string.IsNullOrEmpty(this.FirstName) ||
                string.IsNullOrEmpty(this.LastName) ||
                string.IsNullOrEmpty(this.Password)) return false;

            if (this.ExistsInDatabase()) return false;

            this.Password = this.GetHashedPass();

            return userRepository.AddUser(GetAsDTO(), this.RefreshToken);
        }

        public bool ExistsInDatabase()
        {
            return userRepository.UserExists(this.Email, "email") || userRepository.UserExists(this.Username, "username");
        }

        public bool SetRefreshToken(char[] token)
        {
            if (token == null) return false;
            if (token.Length < 255) return false;

            this.RefreshToken = token;
            return true;
        }

        public bool Update(string type)
        {
            if (type.Equals("refreshToken"))
            {
                if (this.ExistsInDatabase())
                {
                    return userRepository.UpdateRefreshToken(this.Email, GetHashedPass(), this.RefreshToken);
                }
            }

            return false;
        }

        private string GetHashedPass()
        {
            string salt = Salt.CreateSalt(this.Email);
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(this.Email + this.Password));
            string hashedPass = Salt.ByteArrayToString(hashedDataBytes);
            return hashedPass;
        }

        public UserDTO GetAsDTO()
        {
            return new UserDTO {
                Email = this.Email,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Password = this.Password,
                UserName = this.Username};
        }
    }
}
