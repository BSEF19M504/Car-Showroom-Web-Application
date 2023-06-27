using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Car_Showroom_Web_Application.Models
{
    public class UserDAO
    {

        private static string connectionString = "server=127.0.0.1;user id=abc;password=1234root;database=ead";

        public static bool IsUserAvailable(string email)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            //Preventing Injection
            //using Parameters
            string query = "SELECT id FROM emp WHERE email = @Email";
            MySqlParameter p1 = new MySqlParameter("Email", email);

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.Add(p1);

            connection.Open();
            //cmd.ExecuteNonQuery();
            MySqlDataReader reader = cmd.ExecuteReader();
            //int item = (int)cmd.ExecuteScalar();
            bool item = reader.HasRows;
            connection.Close();
            return item;
            //return reader.Read();

        }
        public static User GetUser(string email, string password)
        {
            bool available = IsUserAvailable(email);

            
            if (available)
            {
                MySqlConnection connection = new MySqlConnection(connectionString);

                string query = "SELECT * FROM emp WHERE email = @Email AND pass = @Password";
                MySqlParameter p1 = new MySqlParameter("Email", email);
                MySqlParameter p2 = new MySqlParameter("Password", password);

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                connection.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    User user = new User
                    {
                        Id = reader.GetInt32("id"),
                        UserName = reader.GetString("name"),
                        Email = reader.GetString("email"),
                        Password = reader.GetString("pass")
                    };
                    connection.Close();
                    return user;
                }
                else
                {
                    connection.Close();
                    return new User()
                    {
                        Id = -1
                    };
                }
            }
            else
            {
                return null;
            }

        }

        public static User GetUserById(int id)
        { 
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT * FROM emp WHERE id = @ID";
            MySqlParameter p = new MySqlParameter("ID", id);

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.Add(p);
            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                User user = new User
                {
                    Id = reader.GetInt32("id"),
                    UserName = reader.GetString("name"),
                    Email = reader.GetString("email"),
                    Password = reader.GetString("pass")
                };
                connection.Close();
                return user;
            }
            else
            {
                connection.Close();
                return new User()
                {
                    Id = -1,
                    UserName = "Not Found"
                };
            }

        }
    }
}