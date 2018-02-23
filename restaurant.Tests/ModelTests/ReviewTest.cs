using Microsoft.VisualStudio.TestTools.UnitTesting;
using restaurantProject.Models;
using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace restaurantProject.Tests
{
    [TestClass]
    public class ReviewTests : IDisposable
    {
        public void Dispose()
        {
            Review.DeleteAll();
            Restaurant.DeleteAll();
        }

        public ReviewTests()
        {
            DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=best_restaurants_test;";
        }

        [TestMethod]
        public void Save_SavesToDatabase_Review()
        {
            Restaurant tempRestaurant = new Restaurant("Justin's Jerkeria", "Chicago", 3);
            tempRestaurant.Save();

            Review tempReview = new Review(4, "Best restaurant", "12-30-2017", "Justin", tempRestaurant.GetId());
            tempReview.Save();

            List<Review> testResult = new List<Review>{tempReview};
            List<Review> revs = tempRestaurant.GetReviews();

            CollectionAssert.AreEqual(testResult, revs);
        }

        [TestMethod]
        public void Find_FindCorrectReview_True()
        {
            Restaurant tempRestaurant = new Restaurant("Justin's Jerkeria", "Chicago", 3);
            tempRestaurant.Save();

            Review tempReview = new Review(4, "Best restaurant", "12-30-2017", "Justin", tempRestaurant.GetId());
            tempReview.Save();

            Assert.AreEqual(tempReview, Review.Find(tempReview.GetId()));
        }

        [TestMethod]
        public void Delete_DeletesReviewFromDatabase_True()
        {
            Restaurant tempRestaurant = new Restaurant("Justin's Jerkeria", "Chicago", 3);
            tempRestaurant.Save();

            Review tempReview = new Review(4, "Best restaurant", "12-30-2017", "Justin", tempRestaurant.GetId());
            tempReview.Save();

            tempReview.Delete();
            Assert.AreEqual(0, tempRestaurant.GetReviews().Count);
        }
    }
}
