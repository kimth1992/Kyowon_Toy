using Kyowon_Toy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kyowon_Toy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Test(string x, string y)
        {
            ViewData["x"] = x;
            ViewBag.y = y;

            List<TestModel> list = new List<TestModel>();
            list.Add(new TestModel() { x = 1, y = "a" });
            list.Add(new TestModel() { x = 2, y = "b" });
            list.Add(new TestModel() { x = 3, y = "c" });
            list.Add(new TestModel() { x = 4, y = "d" });

            ViewData["list"] = list;

            return View(list);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
    }
}
