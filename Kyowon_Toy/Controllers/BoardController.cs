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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using Microsoft.Win32;
using System.Net;
using System.Net.Http.Headers;
using KyowonToy.lib.DataBase;

namespace Kyowon_Toy.Controllers
{
    public class BoardController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }


        private readonly ILogger<BoardController> _logger;
        private IWebHostEnvironment Environment;

        public BoardController(ILogger<BoardController> logger, IWebHostEnvironment _environment)
        {
            _logger = (ILogger<BoardController>)logger;
            Environment = _environment;
        }




        public IActionResult BoardList(int page, string key, string keyword)
        {


            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

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



            for(int i = 0; i < boards.Count; i++)
            {
                boards[i].commentList = CommentModel.CommentAll(boards[i].idx);
                boards[i].fileList = FileModel.FlieAll(boards[i].idx);
            }


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
            ViewBag.member = member;

            //return View(BoardModel.GetList(search));
            return View(answer);

        }


        [Authorize]
        public IActionResult BoardWrite()
        {
            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

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
            return View();
        }


        /*[HttpPost]
        public IActionResult Index(List<IFormFile> postedFiles)
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += fileName + ",";
                }
            }
            return View();
        }*/

        // 관리자만 사용 할 수 있다.
        //[Authorize(Roles ="admin")]
       
       

        [Authorize]
        public IActionResult BoardView(int idx)
        {
            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

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

            BoardModel board;
            List<CommentModel> comments = CommentModel.CommentAll(idx);
            List<LikeModel> likeList = LikeModel.findBoardCount(idx);

           


            LikeModel likeCheck = LikeModel.findBoardLike(idx, member_seq);

            if (likeCheck == null)
            {
                ViewBag.likeOrNot = 0;
            } else
            {
                ViewBag.likeOrNot = 1;
            }


      

            board = BoardModel.Get(idx);

            MemberModel writer = MemberModel.findByNo(board.user);
     
            

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

            CheckModel check =  CheckModel.findCheck(idx, member_seq);
            if(check == null)
            {
                CheckModel checkModel = new CheckModel();
                checkModel.board_idx = idx;
                checkModel.member_seq = member_seq;
                checkModel.registeredDate = DateTime.Now;
                checkModel.Insert();
            }

            List<FileModel> files = FileModel.FlieAll(board.idx);
       
       


            ViewBag.files = files;
            ViewBag.member = member;
            ViewBag.writer = writer;
            ViewBag.NextBoard = nextBoard;
            ViewBag.preBoard = preBoard;

            ViewBag.comments = comments;
            ViewBag.likeList = likeList;
            ViewBag.likeCheck = likeCheck;


            return View(board);
        }


        public IActionResult BoardCheck(int idx)
        {

            List<CheckModel> checks = CheckModel.CheckAll(idx);
            List<MemberModel> members = new List<MemberModel>();

            for(int i = 0; i < checks.Count; i++)
            {
                int num = checks[i].member_seq;
                MemberModel member = MemberModel.findByNo(num);
                members.Add(member);
            }


            ViewBag.checks = checks;
            ViewBag.members = members;

            return PartialView();
        }



        [Authorize]
        public IActionResult BoardEdit(int idx, string type)
        {

            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

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
            List<FileModel> files = FileModel.FlieAll(idx);
            ViewBag.uncheckedMail = count;
            ViewBag.member = member;
            ViewBag.files = files;

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

        public IActionResult BoardWrite_Input(string title, string contents, List<IFormFile> postedFiles)
        {


            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<FileModel> uploadedFiles = new List<FileModel>();
            foreach (IFormFile postedFile in postedFiles)
            {
                FileModel filemodel = new FileModel();
                string fileName = Path.GetFileName(postedFile.FileName);
                filemodel.fileName = fileName;
                filemodel.fileUrl = path;

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(filemodel);
                }
            }
            // path -> 파일경로 : wwwroot\uploads 까지
            // fileName은 실제 파일 이름



            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

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

            var model = new BoardModel();

            model.title = title;
            model.contents = contents;
            model.user = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            model.userName = User.Identity.Name;
            model.Insert();


            BoardModel boardmodel = BoardModel.findByTile(title);

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                FileModel file = uploadedFiles[i];
                file.board_idx = boardmodel.idx;
                file.Insert();
            }




            return Redirect("/board/boardlist");
        }



        [Authorize]
        public IActionResult BoardEdit_Input(int idx, string title, string contents, List<IFormFile> postedFiles)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<FileModel> uploadedFiles = new List<FileModel>();
            foreach (IFormFile postedFile in postedFiles)
            {
                FileModel filemodel = new FileModel();
                string fileName = Path.GetFileName(postedFile.FileName);
                filemodel.fileName = fileName;
                filemodel.fileUrl = path;

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(filemodel);
                }
            }




            // idx가 안넘어옴
            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

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

            var model = BoardModel.Get(idx);

            var userSeq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));


            if (model.user != userSeq)
            {
                throw new Exception("수정 할 수 없습니다.");
            }

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                FileModel file = uploadedFiles[i];
                file.board_idx = model.idx;
                file.Insert();
            }




            model.title = title;
            model.contents = contents;

            model.Update();

            return Redirect("/board/boardlist");
        }

        public ActionResult DownloadFile(int idx, string fileName)
        {
            FileModel file = FileModel.findByFileName(idx, fileName);

            string fullName = file.fileUrl + "\\"   + file.fileName;
            Debug.WriteLine("ㅡㅡㅡㅡㅡㅡㅡㅡㅡ");
            Debug.WriteLine(fullName);

            byte[] fileBytes = GetFile(fullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.fileName);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }


    }
}


