using Auth.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Database
{
    public class SetupDatabase
    {
        public static void Setup()
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string users =
                    "CREATE TABLE IF NOT EXISTS users (" +
                        "id CHAR(38) PRIMARY KEY," +
                        "username VARCHAR(32) NOT NULL UNIQUE," +
                        "sessionID VARCHAR(255)" +
                    ");";

                conn.RunCommand(users);

                string messages =
                    "CREATE TABLE IF NOT EXISTS messages (" +
                        "id CHAR(38) PRIMARY KEY," +
                        "username VARCHAR(32) NOT NULL," +
                        "toUser VARCHAR(32) NOT NULL," +
                        "content TEXT(2000) NOT NULL," +
                        "created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP" +
                    ");";

                conn.RunCommand(messages);

                conn.Close();
            }
        }
    }
}
