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
using KyowonToy.lib.DataBase;

namespace Kyowon_Toy.Controllers
{
    public class LikeController : Controller
    {
        public IActionResult Index()
        {
        
            return View();
        }



        // 게시판 번호와 멤버 번호를 받아와서 좋아요 0,1 만들기
        public IActionResult LikeUpdate(int idx)
        {

            BoardModel board;
            board = BoardModel.Get(idx);

            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            LikeModel likeCheck = LikeModel.findBoardLike(idx, member_seq);

            if (likeCheck == null)
            {
               LikeModel like = new LikeModel();
                like.board_idx = idx;
                like.member_seq = member_seq;
               
                like.Insert();
                return Redirect("/board/boardview?idx=" + idx);
            }
            else
            {
                LikeModel like = new LikeModel();

                like.board_idx = idx;
                like.member_seq = member_seq;
                like.Delete();

                
                
                
                return Redirect("/board/boardview?idx=" + idx);
            }


        }

    }
}


