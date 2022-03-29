using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.Login;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {


        public AdminController()
        {

        }

        // 이런것도 가능함
        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult GetCheck()
        {
            if (User.IsInRole("ADMIN"))
            {
                return Json(new { a = 9 });
            }

            return Json(new { a = 1 });
        }

        [AllowAnonymous]
        public IActionResult GetUserCheck()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Json(new { a = 9 });
            }

            return Json(new { a = 1 });
        }

    }
}

     