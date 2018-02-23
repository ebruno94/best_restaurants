using Microsoft.VisualStudio.TestTools.UnitTesting;
using restaurantProject.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace restaurantProject.Tests
{
    [TestClass]
    public class RestaurantTests : IDisposable
    {
        public void Dispose()
        {
            Review.DeleteAll();
            Restaurant.DeleteAll();
        }

        public RestaurantTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_test;";
        }

        [TestMethod]
        public void GetAll_DatabaseEmptyAtFirst_0()
        {
            //Arrange, Act
            int result = Restaurant.GetAll().Count;

            //Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Save_SavesToDatabase_RestaurantList()
        {
            //Arrange
            Restaurant testRest = new Restaurant("Justin's Jerkeria", "Philly", 3);

            //Act
            testRest.Save();
            List<Restaurant> restos = Restaurant.GetAll();
            List<Restaurant> testRestos = new List<Restaurant>{testRest};

            //Assert
            CollectionAssert.AreEqual(testRestos, restos);
        }

        [TestMethod]
        public void Find_FindItemsInDatabase_Item()
        {
            Restaurant testRest = new Restaurant("Justin's Jerkeria", "Philly", 3);
            testRest.Save();

            Restaurant foundRest = Restaurant.Find(testRest.GetId());

            Assert.AreEqual(testRest, foundRest);
        }
    }
}
