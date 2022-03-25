using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
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
    public class LoginController : Controller
    {
       

        public LoginController()
        {
           
        }

        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        [Route("/login/login")]
        public async Task<IActionResult> LoginProc([FromForm]UserModel input)
        {
            try
            {
                input.ConvertPassword();
                var user = input.GetLoginUser();

                // 로그인 작업

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.user_seq.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.user_name));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.email));
                identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));


                if (user.user_name == "okok")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "ADMIN"));
                }
                
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = false,   // 영속성, 브라우저 종료시 삭제
                    ExpiresUtc = DateTime.UtcNow.AddHours(4), // 쿠키는 4시간의 유지기간 가진다.
                    AllowRefresh = true        // 자동 갱신된다.
            });
                


                return Redirect("/");
            }
            catch (Exception ex)
            {
            return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }

        public IActionResult Register(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        [HttpPost]
        [Route("/login/register")]
        public IActionResult RegisterProc([FromForm] UserModel input)
        {
            try
            {
             
                string password2 = Request.Form["password2"];

                if(input.password != password2)
                {
                    throw new Exception("password와 password2가 다릅니다.");
                }

                input.ConvertPassword();

                // 성공
                input.Register();

                return Redirect("/login/login");
            }
            catch (Exception ex)
            {
                // 실패
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");

            }
        }


        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
