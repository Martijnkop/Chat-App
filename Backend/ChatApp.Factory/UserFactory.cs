using ChatApp.Database;
using ChatApp.Interface.User;

namespace ChatApp.Factory
{
    public class UserFactory
    {
        public static IUser CreateUser()
        {
            return new UserRepository();
        }

        public static IUserContainer CreateUserContainer()
        {
            return new UserRepository();
        }
    }
}