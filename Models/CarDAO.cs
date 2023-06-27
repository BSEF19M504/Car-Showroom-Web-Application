using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Car_Showroom_Web_Application.Models
{
    public class CarDAO
    {

        private static string connectionString = "server=127.0.0.1;user id=abc;password=1234root;database=ead";

        public static Car getCarById(int id)
        {
            List<Car> ls = new List<Car>();

            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT car_id,make,model,year,mileage,price,user_id FROM cars WHERE car_id = @ID";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlParameter p1 = new MySqlParameter("ID", id);

            cmd.Parameters.Add(p1);

            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            Car c = null;
            if (reader.HasRows)
            {
                reader.Read();
                c = new Car()
                {
                    Id = reader.GetInt32("car_id"),
                    Make = reader.GetString("make"),
                    Model = reader.GetString("model"),
                    Year = reader.GetInt32("year"),
                    Mileage = reader.GetInt32("mileage"),
                    Price = reader.GetDouble("price"),
                    UserId = reader.GetInt32("user_id")
                };
            }
            return c;
        }

        internal static int GetCarId(string name)
        {
            string[] token = name.Split(' ');
            string make = token[0];
            string model = token[1];

            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT car_id FROM cars WHERE LOWER(make) LIKE @Make AND LOWER(model) LIKE @Model;";
            MySqlParameter p1 = new MySqlParameter("Make", make.ToLower() + "%");
            MySqlParameter p2 = new MySqlParameter("Model", model.ToLower() + "%");

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);

            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            int id = -1;
            if (reader.HasRows)
            {
                reader.Read();
                id = reader.GetInt32("car_id");
            }

            connection.Close();
            return id;
        }

        public static List<Car> GetCars()
        {
            List<Car> ls = new List<Car>();

            MySqlConnection connection = new MySqlConnection(connectionString);
            
            string query = "SELECT car_id,make,model,year,mileage,price,user_id FROM cars";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Car c = new Car()
                    {
                        Id = reader.GetInt32("car_id"),
                        Make = reader.GetString("make"),
                        Model = reader.GetString("model"),
                        Year = reader.GetInt32("year"),
                        Mileage = reader.GetInt32("mileage"),
                        Price = reader.GetDouble("price"),
                        UserId = reader.GetInt32("user_id")
                    };
                    ls.Add(c);
                }
            }
            connection.Close();
            return ls;
        }

        public static List<string> GetCarsByName(string name)
        {
            List<string> ls = new List<string>();

            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT make,model FROM cars WHERE LOWER(make) LIKE LOWER(@Name) OR LOWER(model) LIKE LOWER(@Name)";
            MySqlParameter p = new MySqlParameter("Name", name.ToLower() + "%");

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Parameters.Add(p);

            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string c = reader.GetString("make") + " " + reader.GetString("model");
                    ls.Add(c);
                }
            }
            connection.Close();
            return ls;
        }

        public static int InsertCar(Car c)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            string insert = "INSERT INTO cars(make,model,year,mileage,user_id,price) VALUES(@Make,@Model,@Year,@Mileage,@User,@Price)";

            MySqlParameter p1 = new MySqlParameter("Make", c.Make);
            MySqlParameter p2 = new MySqlParameter("Model", c.Model);
            MySqlParameter p3 = new MySqlParameter("Year", c.Year);
            MySqlParameter p4 = new MySqlParameter("Mileage", c.Mileage);
            MySqlParameter p5 = new MySqlParameter("User", c.UserId);
            MySqlParameter p6 = new MySqlParameter("Price", c.Price);

            MySqlCommand cmd = new MySqlCommand(insert, connection);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);

            connection.Open();
            int rows = cmd.ExecuteNonQuery();
            connection.Close();

            return rows;
        }

        public static int UpdateCar(Car c)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            string update = "UPDATE cars SET make = @Make, model = @Model, year = @Year, mileage = @Mileage, user_id = @User, price = @Price WHERE car_id = @ID";

            MySqlParameter p1 = new MySqlParameter("Make", c.Make);
            MySqlParameter p2 = new MySqlParameter("Model", c.Model);
            MySqlParameter p3 = new MySqlParameter("Year", c.Year);
            MySqlParameter p4 = new MySqlParameter("Mileage", c.Mileage);
            MySqlParameter p5 = new MySqlParameter("User", c.UserId);
            MySqlParameter p6 = new MySqlParameter("Price", c.Price);
            MySqlParameter p7 = new MySqlParameter("ID", c.Id);

            MySqlCommand cmd = new MySqlCommand(update, connection);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.Parameters.Add(p7);

            connection.Open();
            int rows = cmd.ExecuteNonQuery();
            connection.Close();

            return rows;
        }

        public static int SellCar(int id)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            Car c = getCarById(id);

            if (c == null)
            {
                return -3;
            }
            string delete = "DELETE FROM cars WHERE car_id = @ID";
            MySqlParameter p = new MySqlParameter("ID", id);

            MySqlCommand cmd = new MySqlCommand(delete, connection);
            cmd.Parameters.Add(p);

            connection.Open();
            int rows = cmd.ExecuteNonQuery();

            if (rows != 1)
            {
                return -2;
            }

            string insert = "INSERT INTO sold(car_id,make,model,year,mileage,date_of_sale,price) VALUES(@Id,@Make,@Model,@Year,@Mileage,@Date,@Price)";

            MySqlParameter p1 = new MySqlParameter("Make", c.Make);
            MySqlParameter p2 = new MySqlParameter("Model", c.Model);
            MySqlParameter p3 = new MySqlParameter("Year", c.Year);
            MySqlParameter p4 = new MySqlParameter("Mileage", c.Mileage);
            MySqlParameter p5 = new MySqlParameter("Id", c.Id);
            MySqlParameter p6 = new MySqlParameter("Price", c.Price);
            MySqlParameter p7 = new MySqlParameter("Date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            cmd = new MySqlCommand(insert, connection);
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.Parameters.Add(p5);
            cmd.Parameters.Add(p6);
            cmd.Parameters.Add(p7);

            rows = cmd.ExecuteNonQuery();
            connection.Close();

            return rows;
        }

        public static Dictionary<Car,DateTime> GetSoldCars()
        {
            List<int> deletable = new List<int>();
            Dictionary<Car,DateTime> ls = new Dictionary<Car, DateTime>();

            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT transaction_id,make,model,year,mileage,price,date_of_sale FROM sold";

            MySqlCommand cmd = new MySqlCommand(query, connection);

            connection.Open();
            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Car c = new Car()
                    {
                        Make = reader.GetString("make"),
                        Model = reader.GetString("model"),
                        Year = reader.GetInt32("year"),
                        Mileage = reader.GetInt32("mileage"),
                        Price = reader.GetDouble("price")
                    };
                    DateTime dt = reader.GetDateTime("date_of_sale");
                    if (dt.AddMonths(3) > DateTime.Now)
                    {
                        ls.Add(c, dt);
                    }
                }
            }
            connection.Close();
            return ls;
        }
    }
}