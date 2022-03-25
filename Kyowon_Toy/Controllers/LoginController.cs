using Kyowon_Toy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Kyowon_Toy.Controllers
{
    public class LoginController : Controller
    {


        public LoginController()
        {

        }

        public IActionResult Index()
        {
            return Redirect("/login/login");
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}

      