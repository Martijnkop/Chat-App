using System;
using MySql.Data.MySqlClient;

namespace Auth.Database
{
    public class DBConnection
    {
        internal MySqlConnection Connection;
        private string Server { get; set; } = "127.0.0.1";
        private string Database { get; set; } = "chatapp";
        private string Username { get; set; } = "root";
        private string Password { get; set; } = "3iYvGvz9";

        public DBConnection()
        {
            Connection = new MySqlConnection($"SERVER={this.Server};DATABASE={this.Database};UID='{this.Username}';PASSWORD='{this.Password}';SSL Mode=None");
        }

        internal bool Open()
        {
            try
            {
                Connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        Console.WriteLine(e.Message);
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        internal bool Close()
        {
            try
            {
                Connection.Close();
                return true;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        internal void RunCommand(string command)
        {
            MySqlCommand cmd = new(command, this.Connection);
            cmd.ExecuteNonQuery();
        }
    }
}