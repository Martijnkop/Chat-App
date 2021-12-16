using Auth.Database;
using ChatApp.Interface.User;
using MySql.Data.MySqlClient;

namespace ChatApp.Database
{
    public class UserRepository : IUser, IUserContainer
    {
        public bool AddUserToDatabase(UserDTO dto)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string cmd =
                    "INSERT INTO users (" +
                        "id," +
                        "username," +
                        "sessionID" +
                    ") values(" +
                        $"'{Guid.NewGuid()}'," +
                        $"'{dto.Username}'," +
                        $"'0'" +
                    ");";

                conn.RunCommand(cmd);

                conn.Close();

                return true;
            }

            return false;
        }

        public UserDTO FindByUsername(string username)
        {
            DBConnection conn = new();

            if (conn.Open())
            {
                string getIngredient =
                    $"SELECT * FROM users WHERE username='{username}';";

                MySqlCommand cmd = new(getIngredient, conn.Connection);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string u = reader.GetString(1);
                        string connectionID = reader.GetString(2);

                        conn.Close();
                        return new UserDTO { Username = u, ConnectionID = connectionID };
                    }
                }
            }

            return null;
        }

        public void SetConnectionID(string username, string connectionID)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string updateRefreshToken =
                    $"UPDATE users SET sessionID='{connectionID}' WHERE username='{username}';";

                conn.RunCommand(updateRefreshToken);

                conn.Close();
            }
        }

        public List<UserDTO> GetAllUsers()
        {
            DBConnection conn = new();

            List<UserDTO> users = new();

            if (conn.Open())
            {
                string getAll =
                    "SELECT * FROM users ORDER BY username;";

                MySqlCommand cmd = new(getAll, conn.Connection);

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        string username = reader.GetString(1);
                        string connID = reader.GetString(2);

                        users.Add(new UserDTO { Username = username, ConnectionID = connID });
                    }
                }

                conn.Close();
            }

            return users;
        }
    }
}