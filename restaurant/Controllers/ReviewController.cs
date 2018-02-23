using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using restaurantProject.Models;

namespace restaurantProject.Controllers
{
    public class ReviewController : Controller
    {
        [HttpPost("/Review/Create")]
        public ActionResult Create()
        {
            string reviewer = Request.Form["reviewName"];
            string date = Request.Form["reviewDate"];
            int rating = Int32.Parse(Request.Form["reviewRate"]);
            string description = Request.Form["reviewDesc"];
            int restaurantId = Int32.Parse(Request.Form["restaurantId"]);
            Review myReview = new Review(rating, description, date, reviewer, restaurantId);
            myReview.Save();
            return RedirectToAction("Info", "Restaurant", new {id = restaurantId});
        }

        [HttpGet("/Review/Delete/{id}")]
        public ActionResult Delete(int id)
        {
            Review myReview = Review.Find(id);
            myReview.Delete();
            return RedirectToAction("Info", "Restaurant", new {id = myReview.GetRestaurantId()});
        }
    }
}
