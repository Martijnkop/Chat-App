using Auth.Database.User;
using Auth.Interface.User;
using System;

namespace Auth.Factory
{
    public class UserFactory
    {
        public static IUserRepository CreateUserRepository()
        {
            return new UserRepository();
        }

        public static IUser CreateUser()
        {
            return new UserRepository();
        }
    }
}
