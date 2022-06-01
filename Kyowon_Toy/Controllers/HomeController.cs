using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Kyowon_Toy.Models;
using Microsoft.AspNetCore.Hosting;

namespace Kyowon_Toy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment Environment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _environment)
        {
            _logger = logger;
            Environment = _environment;

           

        }

        public IActionResult Index()
        {
            List<MailBoxModel> senders = new List<MailBoxModel>(); // 메일박스 중 로그인 유저한테 온 메일들
            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();

            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

            // 읽지 않은 메일 카운팅 하기
            int count = 0;
            if (member != null)
            {
                for (int i = 0; i < mailBoxList.Count; i++)
                {
                    if (mailBoxList[i].receiver_idx.Equals(member.member_seq))
                    {
                        if (mailBoxList[i].checked_time.Year == 0001)
                        {
                            count++; // 안읽은 메일 수
                        }
                    }
                }
            }

            ViewBag.uncheckedMail = count;
            ViewBag.member = member;
            ViewBag.mailBoxList = mailBoxList;

            return View();
        }

        public IActionResult TicketList()
        {

            return View();
        }


        

        public IActionResult Chat()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
