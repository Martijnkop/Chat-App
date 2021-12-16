using Auth.Interface.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Database.User
{
    public class UserRepository : IUserRepository, IUser
    {

        public bool AddUser(UserDTO dto, char[] refreshToken)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string addIngredient =
                    "INSERT INTO users (" +
                        "id," +
                        "email," +
                        "username," +
                        "firstname," +
                        "lastname," +
                        "password," +
                        "refreshtoken" +
                    ") values(" +
                        $"'{Guid.NewGuid()}'," +
                        $"'{dto.Email}'," +
                        $"'{dto.UserName}'," +
                        $"'{dto.FirstName}'," +
                        $"'{dto.LastName}'," +
                        $"'{dto.Password}'," +
                        $"'{refreshToken}'" +
                    ");";

                conn.RunCommand(addIngredient);

                conn.Close();
                return true;
            }

            return false;
        }

        public bool LogoutUserByRefreshToken(char[] refreshToken)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string updateRefreshToken =
                    $"UPDATE users SET refreshtoken='' WHERE refreshtoken='{refreshToken}'";

                conn.RunCommand(updateRefreshToken);

                conn.Close();
                return true;
            }

            return false;
        }

        public UserDTO GetUser(AccountLogin login)
        {
            DBConnection conn = new();

            if (conn.Open())
            {
                string getIngredient =
                    $"SELECT * FROM users WHERE email='{login.Email}' AND password='{login.Password}';";

                MySqlCommand cmd = new(getIngredient, conn.Connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid id = Guid.Parse(reader.GetString(0));
                        string email = reader.GetString(1);
                        string username = reader.GetString(2);
                        string firstname = reader.GetString(3);
                        string lastname = reader.GetString(4);

                        conn.Close();
                        return new UserDTO { Id = id, Email = email, UserName = username, FirstName = firstname, LastName = lastname };
                    }
                }
            }

            return new UserDTO();
        }

        public bool RefreshTokenExists(char[] refreshToken)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string checkExists =
                    $"SELECT COUNT(users.firstname) FROM users WHERE refreshToken='{refreshToken}';";

                MySqlCommand cmd = new(checkExists, conn.Connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) {
                        int count = reader.GetInt32(0);
                        conn.Close();
                        return count > 0;
                    }
                }
            }

            return false;
        }

        public bool UpdateRefreshToken(string email, string password, char[] refreshToken)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string updateRefreshToken =
                    $"UPDATE users SET refreshtoken='{refreshToken}' WHERE email='{email}' AND password='{password}'";

                conn.RunCommand(updateRefreshToken);

                conn.Close();
                return true;
            }

            return false;
        }

        public bool UserExists(string token, string type)
        {
            DBConnection conn = new();

            if (conn.Open())
            {
                string getIngredient =
                    $"SELECT COUNT(id) FROM users WHERE {type}='{token}';";

                MySqlCommand cmd = new(getIngredient, conn.Connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int amount = int.Parse(reader.GetString(0));

                        conn.Close();
                        return (amount >= 1);
                    }
                }
            }

            return true;
        }

        public UserDTO GetUserByRefreshToken(char[] refreshToken)
        {
            DBConnection conn = new();

            if (conn.Open())
            {
                string getIngredient =
                    $"SELECT * FROM users WHERE refreshToken='{refreshToken}';";

                MySqlCommand cmd = new(getIngredient, conn.Connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Guid id = Guid.Parse(reader.GetString(0));
                        string email = reader.GetString(1);
                        string username = reader.GetString(2);
                        string firstname = reader.GetString(3);
                        string lastname = reader.GetString(4);

                        conn.Close();
                        return new UserDTO { Id = id, Email = email, UserName = username, FirstName = firstname, LastName = lastname };
                    }
                }
            }

            return null;
        }
    }
}
