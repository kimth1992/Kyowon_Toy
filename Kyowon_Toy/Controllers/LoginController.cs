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

namespace Kyowon_Toy.Controllers
{
    public class LoginController : Controller
    {

      
        public LoginController()
        {

        }


        public IActionResult UserInfo()
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


        public IActionResult Update(int member_seq, string department, string position, string mobile_tel, string office_tel, string email, string mainwork)
        {
            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();

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

            member.department = department;
            member.position = position;
            member.mobile_Tel = mobile_tel;
            member.email = email;
            member.office_Tel = office_tel;
            member.mainwork = mainwork;

            member.Update();


            ViewBag.uncheckedMail = count;
            ViewBag.member = member;
            return Redirect("/login/userinfo");
        }

        public IActionResult updatePhotoForm(ICollection<IFormFile> photo,
            [FromServices] IWebHostEnvironment environment)
        {

            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

            string uploadDirectoryPath = Path.Combine(environment.WebRootPath, "upload");
            string uploadDirectoryPath2 = Path.Combine(environment.EnvironmentName);

            long totalSize = 0L;

            if (photo.Count != 0)
            {

                foreach (IFormFile formFile in photo)
                {
                    string uploadFilePath = Path.Combine(uploadDirectoryPath, formFile.FileName);
                    member.photo = formFile.FileName;
                    member.Update();

                    using (FileStream fileStream = System.IO.File.Create(uploadFilePath))
                    {
                        formFile.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                    using (FileStream fileStream = System.IO.File.Create(uploadDirectoryPath2))
                    {
                        formFile.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                    totalSize += formFile.Length;

                }
            }



            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();

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


            member.Update();


            ViewBag.uncheckedMail = count;
            ViewBag.member = member;
            return PartialView();
        }



        public IActionResult Login(string msg)
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
                // identity.AddClaim(new Claim(ClaimTypes.LoginUser, member));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, member.member_seq.ToString()));
               identity.AddClaim(new Claim(ClaimTypes.Name, member.name));
               //identity.AddClaim(new Claim(ClaimTypes.Email, member.Email));
               identity.AddClaim(new Claim("LastCheckDateTime", DateTime.UtcNow.ToString("yyyyMMddHHmmss")));


                if (member.name == "admin")
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

                ViewBag.member = member;

                return Redirect("/");
            }
            catch (Exception ex)
            {
                return Redirect($"/login/login?msg={HttpUtility.UrlEncode(ex.Message)}");
            }
        }


        public IActionResult Register(string msg)
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

            ViewData["msg"] = msg;
           
            return View();
        }

        /*
        public Image CreateThumbnailImage(string fileName, int width, int height)
        {

            Image image = Image.FromFile(fileName);
            Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
            image.Dispose();
            string thumbname = fileName + "_100x100";
            Debug.WriteLine(thumbname);
            thumb.Save(Path.ChangeExtension(thumbname, "jpg"));

            return thumb;

        }
        */

        public void CreateThumbnail(string imageFile, [FromServices] IWebHostEnvironment environment)
        {


            string uploadDirectoryPath = Path.Combine(environment.WebRootPath, "upload");
        

            string dir = new FileInfo(imageFile).DirectoryName;
           // string thmFilePath = Path.Combine(environment.WebRootPath, "upload", imageFile+"thumbnail.jpeg");
            string thmFilePath = Path.Combine(uploadDirectoryPath ,imageFile + "thumbnail.jpeg");
    

         //   string uploadFilePath = Path.Combine(uploadDirectoryPath, imageFile + "thumbnail.jpeg");

            System.Drawing.Image image = System.Drawing.Image.FromFile(imageFile);
           var thumbImage =  image.GetThumbnailImage(64,64,new Image.GetThumbnailImageAbort(()=>false), IntPtr.Zero);

            thumbImage.Save(thmFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

        }

        public void CreateThumbnail2(string imageFile, [FromServices] IWebHostEnvironment environment)
        {


            string uploadDirectoryPath = Path.Combine(environment.WebRootPath, "upload");


            string dir = new FileInfo(imageFile).DirectoryName;
            // string thmFilePath = Path.Combine(environment.WebRootPath, "upload", imageFile+"thumbnail.jpeg");
            string thmFilePath = Path.Combine(uploadDirectoryPath, imageFile + "thumbnail2.jpeg");


            //   string uploadFilePath = Path.Combine(uploadDirectoryPath, imageFile + "thumbnail.jpeg");

            System.Drawing.Image image = System.Drawing.Image.FromFile(imageFile);
            var thumbImage = image.GetThumbnailImage(120, 120, new Image.GetThumbnailImageAbort(() => false), IntPtr.Zero);

            thumbImage.Save(thmFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);

        }





        [HttpPost]
        [Route("/login/register")]
        public IActionResult RegisterProc(int grade, string name, string password, int birthyy,
            int birthmm, int birthdd, string mobile_tel, string department, string position, ICollection<IFormFile> photo,
            [FromServices] IWebHostEnvironment environment)
        {

            MemberModel member = new MemberModel();

            string uploadDirectoryPath = Path.Combine(environment.WebRootPath, "upload");
            string uploadDirectoryPath2 = Path.Combine(environment.EnvironmentName);

            long totalSize = 0L;

            if (photo.Count != 0)
            {

                foreach (IFormFile formFile in photo)
                {
                    string uploadFilePath = Path.Combine(uploadDirectoryPath, formFile.FileName);
                    member.photo = formFile.FileName;

                    using (FileStream fileStream = System.IO.File.Create(uploadFilePath))
                    {
                        formFile.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                    using (FileStream fileStream = System.IO.File.Create(uploadDirectoryPath2))
                    {
                        formFile.CopyTo(fileStream);
                        fileStream.Flush();
                    }

                    totalSize += formFile.Length;

                }
            }
            else
            {
                member.photo = "doctorinfo.jpg";
            }


            member.name = name;
            member.grade = grade;
            member.mobile_Tel = mobile_tel;
            member.birthDay = new DateTime(birthyy, birthmm, birthdd);
            member.password = password;
            member.department = department;
            member.position = position;
  
            try
            {

                member.ConvertPassword();

                member.Insert();

                MemberModel member2 = MemberModel.Get(member.name);
                member2.email = member2.member_seq + "@Kyowon.co.kr";

                member2.UpdateEmail();
                ViewBag.member = member;

                if (member.photo != "doctorinfo.jpg")
                {
                    CreateThumbnail(member.photo, environment);
                    CreateThumbnail2(member.photo, environment);
                }


                return Redirect("/login/login?msg="+member2.member_seq);
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
