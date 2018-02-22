using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using restaurantProject.Models;

namespace restaurantProject.Controllers
{
    public class RestaurantController : Controller
    {
        [HttpGet("/Restaurant/Form")]
        public ActionResult Form()
        {
            return View(Cuisine.GetAll());
        }

        [HttpPost("/Restaurant/Create")]
        public ActionResult Create()
        {
            string name = Request.Form["restName"];
            string city = Request.Form["restCity"];
            int rating = Int32.Parse(Request.Form["rating"]);
            Console.WriteLine("This is the Request Form result: ");
            Console.WriteLine(Request.Form["cuisine"]);
            IEnumerable<string> myCuisinesString = Request.Form["cuisine"];
            Console.WriteLine(myCuisinesString);
            List<string> myCuisines = new List<string>();
            foreach (var cuisine in myCuisinesString)
            {
                myCuisines.Add(cuisine);
            }
            Restaurant myRestaurant = new Restaurant(name, city, rating);
            myRestaurant.SetCuisineIds(myCuisines);
            myRestaurant.Save();

            return View("Info", myRestaurant);
        }

        [HttpGet("/Restaurant/Info/{id}")]
        public ActionResult Info(int id)
        {
            Restaurant myRestaurant = Restaurant.Find(id);
            return View(myRestaurant);
        }

        [HttpGet("/Restaurant/Clear")]
        public ActionResult Delete()
        {
            Restaurant.DeleteAll();
            return RedirectToAction("Index", "Home");
        }
    }
}
