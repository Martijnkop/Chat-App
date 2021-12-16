using Auth.Database;
using ChatApp.Interface.Message;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Database
{
    public class MessageRepository : IMessage, IMessageContainer
    {
        public void AddToDatabse(MessageDTO dto)
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string cmd =
                    "INSERT INTO messages (" +
                        "id," +
                        "username," +
                        "toUser," +
                        "content" +
                    ") values(" +
                        $"'{Guid.NewGuid()}'," +
                        $"'{dto.Username}'," +
                        $"'{dto.SentTo}'," +
                        $"'{dto.Message}'" +
                    ");";

                conn.RunCommand(cmd);

                conn.Close();

            }

        }

        public List<MessageDTO> GetAllByUsernames(string user, string self)
        {
            DBConnection conn = new();

            List<MessageDTO> users = new();

            if (conn.Open())
            {
                string getAll =
                    $"SELECT * FROM messages WHERE (username='{user}' AND toUser='{self}') OR (username='{self}' AND toUser='{user}') ORDER BY created_at;";

                MySqlCommand cmd = new(getAll, conn.Connection);

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        Guid id = Guid.Parse(reader.GetString(0));
                        string username = reader.GetString(1);
                        string sentTo = reader.GetString(2);
                        string message = reader.GetString(3);

                        users.Add(new MessageDTO { ID = id, Username = username, SentTo = sentTo, Message = message});
                    }
                }

                conn.Close();
            }

            return users;
        }
    }
}
