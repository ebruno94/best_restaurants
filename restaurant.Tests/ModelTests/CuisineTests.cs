using Microsoft.VisualStudio.TestTools.UnitTesting;
using restaurantProject.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace restaurantProject.Tests
{
    [TestClass]
    public class CuisineTest : IDisposable
    {
        public void Dispose()
        {
            Restaurant.DeleteAll();
        }

        public CuisineTest()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_test;";
        }

        [TestMethod]
        public void Find_FindCorrectCuisine_True()
        {
            Cuisine test = new Cuisine("Jerkeria");
            test.Save();

            Assert.AreEqual(test, Cuisine.Find(test.GetId()));
        }
    }
}
