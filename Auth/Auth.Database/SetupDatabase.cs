using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Database
{
    public class SetupDatabase
    {
        public static void Setup()
        {
            DBConnection conn = new();
            if (conn.Open())
            {
                string cmd = 
                    "CREATE TABLE IF NOT EXISTS users (" +
                        "id CHAR(38) PRIMARY KEY," +
                        "email VARCHAR(255) NOT NULL UNIQUE," +
                        "username VARCHAR(32) NOT NULL UNIQUE," +
                        "firstname VARCHAR(32)," +
                        "lastname VARCHAR(64)," +
                        "password VARCHAR(1023)," +
                        "refreshtoken CHAR(255)" +
                    ");";

                conn.RunCommand(cmd);

                conn.Close();
            }
        }
    }
}
