using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace restaurantProject.Models
{
    public class Restaurant
    {
        private string _name;
        private string _city;
        private int _id;
        private int _rating;
        private List<string> _cuisineIds = new List<string>();

        public Restaurant(string name, string city, int rating)
        {
            _name = name;
            _city = city;
            _rating = rating;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetCity()
        {
            return _city;
        }

        public string GetRating()
        {
            return (_rating).ToString();
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public void SetCuisineIds()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cuisines_restaurants WHERE restaurant_id=@restaurant_id;";

            MySqlParameter restaurantId = new MySqlParameter("@restaurant_id", _id);
            cmd.Parameters.Add(restaurantId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                _cuisineIds.Add(rdr.GetInt32(1).ToString());
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void SetCuisineIds(List<string> cuisineIds)
        {
            _cuisineIds = cuisineIds;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO restaurants (name, city, rating) VALUES (@name, @city, @rating)";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = _name;
            cmd.Parameters.Add(name);

            MySqlParameter city = new MySqlParameter();
            city.ParameterName = "@city";
            city.Value = _city;
            cmd.Parameters.Add(city);

            MySqlParameter rating = new MySqlParameter();
            rating.ParameterName = "@rating";
            rating.Value = _rating;
            cmd.Parameters.Add(rating);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            foreach (string cuisineIdString in _cuisineIds)
            {
                Console.WriteLine(cuisineIdString);
                int cuisineId = Int32.Parse(cuisineIdString);
                MySqlCommand command = conn.CreateCommand() as MySqlCommand;
                command.CommandText = @"INSERT INTO cuisines_restaurants (cuisine_id, restaurant_id) VALUES (@cuisine_id, @restaurant_id);";

                MySqlParameter tempCuisine = new MySqlParameter("@cuisine_id", cuisineId);
                command.Parameters.Add(tempCuisine);

                MySqlParameter tempRestaurant = new MySqlParameter("@restaurant_id", this.GetId());
                command.Parameters.Add(tempRestaurant);

                command.ExecuteNonQuery();
                conn.Close();
                conn.Open();
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Restaurant Find(int restId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd= conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM restaurants WHERE id=@id";

            MySqlParameter id = new MySqlParameter("@id", restId);
            cmd.Parameters.Add(id);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            string tempName = "";
            int tempId = 0;
            string tempCity = "";
            int tempRating = 0;

            while (rdr.Read())
            {
                tempId = rdr.GetInt32(0);
                tempName = rdr.GetString(1);
                tempCity = rdr.GetString(2);
                tempRating = rdr.GetInt32(3);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            Restaurant myRestaurant = new Restaurant(tempName, tempCity, tempRating);
            myRestaurant.SetId(tempId);
            myRestaurant.SetCuisineIds();
            return myRestaurant;
        }

        public static List<Restaurant> GetAll()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM restaurants;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string city = rdr.GetString(2);
                int rating = rdr.GetInt32(3);
                restaurants.Add(new Restaurant(name, city, rating));
                restaurants[restaurants.Count-1].SetId(id);
                restaurants[restaurants.Count-1].SetCuisineIds();
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return restaurants;
        }

        public List<Cuisine> GetCuisines()
        {
            List<Cuisine> myCuisines = new List<Cuisine>();

            MySqlConnection conn = DB.Connection();
            conn.Open();

            foreach (string idString in _cuisineIds)
            {
                MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
                cmd.CommandText = @"Select * FROM cuisines WHERE id=@id;";

                int tempId = Int32.Parse(idString);

                MySqlParameter id = new MySqlParameter("@id", tempId);
                cmd.Parameters.Add(id);
                MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

                while (rdr.Read())
                {
                    Cuisine myCuisine = new Cuisine(rdr.GetString(1));
                    myCuisine.SetId(rdr.GetInt32(0));
                    myCuisines.Add(myCuisine);
                }
                conn.Close();
                conn.Open();
            }

            conn.Close();
            if(conn != null)
            {
                conn.Dispose();
            }
            return myCuisines;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM restaurants;DELETE FROM cuisines_restaurants";

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM restaurants WHERE id=@id;";

            MySqlParameter id = new MySqlParameter("@id", _id);
            cmd.Parameters.Add(id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
