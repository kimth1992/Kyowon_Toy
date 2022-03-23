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
            var dt = new DataTable();
            string sex = "남";

            return View(MemberModel.GetList(sex));

   


            /*              using (var cmd = new MySqlCommand())
                          {
                              string sex = "남";
                              cmd.Connection = conn;

                              *//*  cmd.CommandText = @"select m.no
          , m.name
          ,m.sex
          FROM
          member m
          where
          m.sex = @sex";*//*

                              cmd.Parameters.AddWithValue("@sex", sex);

                              var reader = cmd.ExecuteReader();

                              dt.Load(reader);

                              cmd.ExecuteReader();
                              // cmd.ExecuteNonQuery();
                          }*/



            /*  var list = new List<MemberModel>();

              foreach(DataRow row in dt.Rows)
              {
                  var member = new MemberModel();
                  member.No = Convert.ToInt32(row["no"]);
                  member.Name = row["name"] as string;
                  member.Sex = row["sex"] as string;

                  list.Add(member);
              }*/

            // list.Add(new MemberModel() { });





            //ViewData["dt"] = dt;
            /* ViewData["list"] = list;*/

            /*
                        return View();*/
        }


      //  public IActionResult PrivacyInsert(string name, string sex)
        public IActionResult PrivacyInsert([FromForm]MemberModel model)
        {

          /*  var model = new MemberModel();
            model.Name = name;
            model.Sex = sex;
          */
            model.Insert();
            return Redirect("/home/Privacy");
            //return Json(new { msg = "ok" });
           
        }




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
