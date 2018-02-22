using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace restaurantProject.Models
{
    public class Cuisine
   {
       private string _name;
       private int _id;

       public Cuisine(string name)
       {
           _name = name;
       }

       public string GetName()
       {
           return _name;
       }

       public void SetId(int id)
       {
           _id = id;
       }
       public int GetId()
       {
           return _id;
       }

       public void Save()
       {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"INSERT INTO cuisines (name) VALUES (@name);";

           MySqlParameter name = new MySqlParameter();
           name.ParameterName = "@name";
           name.Value = _name;
           cmd.Parameters.Add(name);

           cmd.ExecuteNonQuery();
           _id = (int) cmd.LastInsertedId;

           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
       }

       public static List<Cuisine> GetAll()
       {
           List<Cuisine> cuisines = new List<Cuisine>();

           MySqlConnection conn = DB.Connection();
           conn.Open();
           MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT * FROM cuisines;";

           MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

           while (rdr.Read())
           {
               int id = rdr.GetInt32(0);
               string name = rdr.GetString(1);
               cuisines.Add(new Cuisine(name));
               cuisines[cuisines.Count-1]._id = id;
           }

           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }

           return cuisines;
       }

       public static Cuisine Find(int cuisineId)
       {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"SELECT * FROM cuisines WHERE id=@id;";

           MySqlParameter id = new MySqlParameter();
           id.ParameterName = "@id";
           id.Value = cuisineId;
           cmd.Parameters.Add(id);

           MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

           string tempName = "";
           int tempId = 0;

           while (rdr.Read())
           {
               tempId = rdr.GetInt32(0);
               tempName = rdr.GetString(1);
           }

           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
           Cuisine myCuisine = new Cuisine(tempName);
           myCuisine.SetId(tempId);
           return myCuisine;
       }

    //    public static void DeleteAll()
    //    {
    //        MySqlConnection conn = DB.Connection();
    //        conn.Open();
    //        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
    //        cmd.CommandText = @"DELETE FROM cuisines;";
       //
    //        cmd.ExecuteNonQuery();
       //
    //        conn.Close();
    //        if (conn != null)
    //        {
    //            conn.Dispose();
    //        }
    //    }

       public static void Delete(int cuisineId)
       {
           MySqlConnection conn = DB.Connection();
           conn.Open();
           MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
           cmd.CommandText = @"DELETE FROM cuisines WHERE id=@id;";

           MySqlParameter id = new MySqlParameter();
           id.ParameterName = "@id";
           id.Value = cuisineId;
           cmd.Parameters.Add(id);

           cmd.ExecuteNonQuery();

           conn.Close();
           if (conn != null)
           {
               conn.Dispose();
           }
       }

       public override bool Equals(System.Object otherCuisine)
       {
           if (!(otherCuisine is Cuisine))
           {
               return false;
           }
           else
           {
               Cuisine newCuisine = (Cuisine) otherCuisine;
               bool nameEquality = (_name == newCuisine.GetName());
               return nameEquality;
           }
       }
   }
}
