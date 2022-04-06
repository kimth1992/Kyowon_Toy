using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Kyowon_Toy.Models;
using Kyowon_Toy.Models.Login;
using System.Security.Claims;

namespace Kyowon_Toy.Controllers
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
        public async Task<IActionResult> LoginProc([FromForm]MemberModel input)
        {
            try
            {
                    // name = test123 , password = 1234
                input.ConvertPassword();

                var member = input.GetLoginUser();

                

                // 로그인 작업

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, member.Member_seq.ToString()));
               identity.AddClaim(new Claim(ClaimTypes.Name, member.Name));
               //identity.AddClaim(new Claim(ClaimTypes.Email, member.Email));
               identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));


                if (member.Name == "admin")
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

        /*
         *  [HttpPost]
        [Route("/login/register")]
        public IActionResult RegisterProc([FromForm] MemberModel input)
        {

            DateTime birthDay = input.birthyy

            try
            {

     

                input.ConvertPassword();

                // 성공
                input.Insert();

                return Redirect("/login/login");
            }
            catch (Exception ex)
            {
                // 실패
                return Redirect($"/login/register?msg={HttpUtility.UrlEncode(ex.Message)}");

            }
        }
         */
     

        [HttpPost]
        [Route("/login/register")]
        public IActionResult RegisterProc(int grade, string name, string password, int birthyy,
            int birthmm, int birthdd, string mobile_tel, string department)
        {
  
            MemberModel member = new MemberModel();
            member.Name = name;
            member.Grade = grade;
            member.Mobile_Tel = mobile_tel;
            member.BirthDay = new DateTime(birthyy, birthmm, birthdd);
            member.password = password;
            member.Department = department;
  
            try
            {

                member.ConvertPassword();

                member.Insert();

                var member2 = MemberModel.Get(member.Name);
                member2.Email = member2.Member_seq + "@Kyowon.co.kr";

                member2.UpdateEmail();



                // member2.UpdateEmail();




                // 이메일 생성

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
