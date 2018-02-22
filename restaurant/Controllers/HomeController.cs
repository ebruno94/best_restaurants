using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using restaurantProject.Models;

namespace restaurantProject.Controllers
{
    public class HomeController: Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            List<Restaurant> myRestaurants = Restaurant.GetAll();
            return View(myRestaurants);
        }
    }
}
