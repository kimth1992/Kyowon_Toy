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
    public class BoardController : Controller
    {
        public IActionResult Index()
        {
        
            return View();
        }


        public IActionResult BoardList(string search, int page)
        {

            int maxListCount = 5; // 한페이지에 몇개 보여줄지
            int countPage = 10; // 페이지 몇개 보여줄지(1~10)
            List<BoardModel> boards = BoardModel.BoardAll();
            //var boards = BoardModel.BoardAll();

            int totalCount = boards.Count();

            int totalPageCount = totalCount / maxListCount;
            if (totalCount % maxListCount > 0)
            {
                totalPageCount++;
            }

            if(totalPageCount < page)
            {
                page = totalPageCount;
            }

            int startPage = ((page -1) / countPage) * countPage +1;
            int endPage = startPage + countPage - 1;

            if(endPage > totalPageCount)
            {
                endPage = totalPageCount;
            }


         var answer = boards.OrderByDescending(x=>x.registeredDate).Skip((page -1) * maxListCount).Take(maxListCount).ToList();
         

            ViewBag.Page = page;
            ViewBag.TotalCount = totalCount; // 전체 게시글 개수
            ViewBag.MaxListCount = maxListCount; // 한페이지에 몇개 보여줄지
            ViewBag.TotalPageCount = totalPageCount; // 전체 페이지 수
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;

         /*   if(Request.QueryString["page"] != null)
            {
                pageNum = Convert.ToInt32(Request.QueryString["page"]);
            }
         */



            //return View(BoardModel.GetList(search));
            return View(answer);

        }

        [Authorize]
        public IActionResult BoardWrite()
        {
            return View();
        }

        // 관리자만 사용 할 수 있다.
        //[Authorize(Roles ="admin")]
        [Authorize]
        public IActionResult BoardWrite_Input(string title, string contents)
        {
            var model = new BoardModel();

            model.Title = title;
            model.Contents = contents;
            model.User = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.UserName = User.Identity.Name;
            model.Insert();

            return Redirect("/board/boardlist");
        }
        public IActionResult BoardView(uint idx)
        {
            BoardModel board;
            board = BoardModel.Get(idx);
            board.UpdateCount(idx);


            BoardModel nextBoard;
            nextBoard = BoardModel.Next(idx);

            if(nextBoard == null)
            {
                nextBoard = new BoardModel();
                nextBoard.Title = "마지막 게시글 입니다.";
            }

            BoardModel preBoard;
            preBoard = BoardModel.Previous(idx);

            if (preBoard == null)
            {
                preBoard = new BoardModel();
                preBoard.Title = "첫 게시글 입니다.";
            }

            ViewBag.NextBoard = nextBoard;
            ViewBag.preBoard = preBoard;



            return View(board);
        }
        [Authorize]
        public IActionResult BoardEdit(uint idx, string type)
        {
            var board = BoardModel.Get(idx);

            var userSeq = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (board.User != userSeq)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            if (type == "U")
            {
                return View(board);
            }
            else if (type == "D")
            {
                board.Delete();
                return Redirect("/board/boardlist");

            }
            throw new Exception("잘못된 요청입니다.");
        }


        [Authorize]
        public IActionResult BoardEdit_Input(uint idx, string title, string contents)
        {
            // idx가 안넘어옴


            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToUInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (model.User != userSeq)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            model.Title = title;
            model.Contents = contents;

            model.Update();

            return Redirect("/board/boardlist");
        }

    }
}
