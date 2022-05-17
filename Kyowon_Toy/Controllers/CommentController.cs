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


namespace Kyowon_Toy.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
        
            return View();
        }



        public IActionResult CommentAdd(int idx, string content)
        {
        
            CommentModel comment = new CommentModel();
            comment.content = content;
            comment.registeredDate = DateTime.Now;

            comment.member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            comment.commentBoard_idx = idx;

            comment.Insert();

                       

            return Redirect("/board/boardview?idx=" + idx);
        }

        public IActionResult CommentDelete(int idx, int commentIdx)
        {

            CommentModel comment = CommentModel.findByNo(commentIdx);
            comment.Delete(commentIdx);
            



            return Redirect("/board/boardview?idx=" + idx);
        }

    }
}


