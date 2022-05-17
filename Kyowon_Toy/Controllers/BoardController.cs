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

       

        public IActionResult BoardList(int page, string key, string keyword)
        {

            int maxListCount = 10; // 한페이지에 몇개 보여줄지
            int countPage = 10; // 페이지 몇개 보여줄지(1~10)
            List<BoardModel> boards;

            if (keyword != null)
            {
                if (key == "title")
                {
                    boards = BoardModel.SearchTitle(keyword);
                    page = 1;

                }
                else if (key == "username")
                {
                    boards = BoardModel.SearchUsername(keyword);
                    page = 1;
                }
                else if (key == "department")
                {
                    boards = BoardModel.SearchDepartment(keyword);
                    page = 1;
                }
                else
                {
                    boards = null;
                }
            }
            else
            {
                boards = BoardModel.BoardAll();
            }

            //var boards = BoardModel.BoardAll();
       

            int totalCount = boards.Count();

            int totalPageCount = totalCount / maxListCount;
            if (totalCount % maxListCount > 0)
            {
                totalPageCount++;
            }

            if (totalPageCount < page)
            {
                page = totalPageCount;
            }

            int startPage = ((page - 1) / countPage) * countPage + 1;
            int endPage = startPage + countPage - 1;

            if (endPage > totalPageCount)
            {
                endPage = totalPageCount;
            }


            var answer = boards.OrderByDescending(x => x.registeredDate).Skip((page - 1) * maxListCount).Take(maxListCount).ToList();


            ViewBag.Page = page;
            ViewBag.TotalCount = totalCount; // 전체 게시글 개수
            ViewBag.MaxListCount = maxListCount; // 한페이지에 몇개 보여줄지
            ViewBag.TotalPageCount = totalPageCount; // 전체 페이지 수
            ViewBag.StartPage = startPage;
            ViewBag.EndPage = endPage;

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

            model.title = title;
            model.contents = contents;
            model.user = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.userName = User.Identity.Name;
            model.Insert();

            return Redirect("/board/boardlist");
        }

        [Authorize]
        public IActionResult BoardView(int idx)
        {
            BoardModel board;
            List<CommentModel> comments = CommentModel.CommentAll(idx);
            List<LikeModel> likeList = LikeModel.findBoardCount(idx);

            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            LikeModel likeCheck = LikeModel.findBoardLike(idx, member_seq);

            if (likeCheck == null)
            {
                ViewBag.likeOrNot = 0;
            } else
            {
                ViewBag.likeOrNot = 1;
            }


      

            board = BoardModel.Get(idx);
     
            

            if (board == null)
            {
                return Redirect("/board/boardNull");
            }


            board.UpdateCount(idx);


            BoardModel nextBoard;
            nextBoard = BoardModel.Next(idx);

            if (nextBoard == null)
            {
                nextBoard = new BoardModel();
                nextBoard.title = "마지막 게시글 입니다.";
            }

            BoardModel preBoard;
            preBoard = BoardModel.Previous(idx);

            if (preBoard == null)
            {
                preBoard = new BoardModel();
                preBoard.title = "첫 게시글 입니다.";
            }

            ViewBag.NextBoard = nextBoard;
            ViewBag.preBoard = preBoard;

            ViewBag.comments = comments;
            ViewBag.likeList = likeList;
            ViewBag.likeCheck = likeCheck;


            return View(board);
        }




        [Authorize]
        public IActionResult BoardEdit(int idx, string type)
        {
            var board = BoardModel.Get(idx);

            var userSeq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(userSeq != board.user)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            if (type == "U")
            {
                return View(board);
            }
            else if (type == "D")
                if (board.user != userSeq)
                {
                    board.Delete(idx);
                    return Redirect("/board/boardlist");

                }
            throw new Exception("잘못된 요청입니다.");
        }


        [Authorize]
        public IActionResult BoardEdit_Input(int idx, string title, string contents)
        {
            // idx가 안넘어옴


            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (model.user != userSeq)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            model.title = title;
            model.contents = contents;

            model.Update();

            return Redirect("/board/boardlist");
        }

    }
}


