using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace restaurantProject.Models
{
    public class Review
    {
        private int _rating;
        private string _description;
        private string _date;
        private string _reviewer;
        private int _id;
        private int _restaurantId;

        public Review(int rating, string description, string date, string reviewer, int restaurantId)
        {
            _rating = rating;
            _description = description;
            _date = date;
            _reviewer = reviewer;
            _restaurantId = restaurantId;
        }

        public int GetRating()
        {
            return _rating;
        }

        public string GetDescription()
        {
            return _description;
        }

        public string GetDate()
        {
            return _date;
        }

        public string GetReviewer()
        {
            return _reviewer;
        }

        public int GetRestaurantId()
        {
            return _restaurantId;
        }

        public int GetId()
        {
            return _id;
        }

        public void SetId(int id)
        {
            _id = id;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO reviews (rating, description, date, reviewer, restaurant_id) VALUES (@rating, @description, @date, @reviewer, @restaurant_id);";

            MySqlParameter rating = new MySqlParameter("@rating", _rating);
            MySqlParameter description = new MySqlParameter("@description", _description);
            MySqlParameter date = new MySqlParameter("@date", _date);
            MySqlParameter reviewer = new MySqlParameter("@reviewer", _reviewer);
            MySqlParameter restaurantId = new MySqlParameter("@restaurant_id", _restaurantId);
            cmd.Parameters.Add(rating);
            cmd.Parameters.Add(description);
            cmd.Parameters.Add(date);
            cmd.Parameters.Add(reviewer);
            cmd.Parameters.Add(restaurantId);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Review Find(int id)
        {
            int tableRating = 0;
            string tableDescription = "";
            string tableDate = "";
            string tableReviewer = "";
            int tableRestaurantId = 0;
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM reviews WHERE id=@id;";

            MySqlParameter tempId = new MySqlParameter("@id", id);
            cmd.Parameters.Add(tempId);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                tableRating = rdr.GetInt32(1);
                tableDescription = rdr.GetString(2);
                tableDate = rdr.GetString(3);
                tableReviewer = rdr.GetString(4);
                tableRestaurantId = rdr.GetInt32(5);
            }
            Review myReview = new Review(tableRating, tableDescription, tableDate, tableReviewer, tableRestaurantId);
            myReview.SetId(id);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return myReview;
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM reviews WHERE id=@id;";

            MySqlParameter tempId = new MySqlParameter("@id", _id);
            cmd.Parameters.Add(tempId);
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM reviews;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherReview)
        {
            if (!(otherReview is Review))
            {
                return false;
            }
            else
            {
                Review newReview = (Review) otherReview;
                return (newReview.GetReviewer() == this.GetReviewer());
            }
        }
    }
}
