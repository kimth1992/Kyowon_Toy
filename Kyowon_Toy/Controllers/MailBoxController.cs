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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using Microsoft.Win32;
using System.Net;
using System.Net.Http.Headers;
using KyowonToy.lib.DataBase;


namespace Kyowon_Toy.Controllers
{
    public class MailBoxController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly ILogger<MailBoxController> _logger;
        private IWebHostEnvironment Environment;


        public MailBoxController(ILogger<MailBoxController> logger, IWebHostEnvironment _environment)
        {
            _logger = (ILogger<MailBoxController>)logger;
            Environment = _environment;
        }


        public IActionResult MailBoxList()
        {

            List<MailBoxModel> mailBoxList = MailBoxModel.findByAll();
            List<MailBoxModel> senders = new List<MailBoxModel>(); // 메일박스 중 로그인 유저한테 온 메일들

            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            MemberModel member = MemberModel.findByNo(member_seq);

            // 읽지 않은 메일 카운팅 하기
            int count = 0;
            if(member != null)
            {
                for(int i = 0; i < mailBoxList.Count; i++)
                {
                    if (member.member_seq.Equals(mailBoxList[i].receiver_idx))
                    {
                        if (mailBoxList[i].checked_time.Year == 0001)
                        {
                            count++; // 안읽은 메일 수
                        }
                    }
                }
            }



            for(int i = 0; i < mailBoxList.Count; i++)
            {
                if (member.member_seq.Equals(mailBoxList[i].receiver_idx))
                {
                    senders.Add(mailBoxList[i]);
                }
            }

            for(int i = 0; i < senders.Count; i++)
            {
                senders[i].sender = MemberModel.findByNo(senders[i].sender_idx);
            }

 


            ViewBag.uncheckedMail = count;
            ViewBag.member = member;
            ViewBag.mailBoxList = mailBoxList;
            return View();

        }


        [Authorize]
        public IActionResult send(string reciever_seq, string title, string content, List<IFormFile> postedFiles)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<MailBoxFileModel> uploadedFiles = new List<MailBoxFileModel>();
            foreach (IFormFile postedFile in postedFiles)
            {
                MailBoxFileModel filemodel = new MailBoxFileModel();
                string fileName = Path.GetFileName(postedFile.FileName);
                filemodel.fileName = fileName;
                filemodel.fileUrl = path;

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(filemodel);
                }
            }



       



            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            
            MailBoxModel mail = new MailBoxModel();
            mail.title = title;
            mail.content = content;
            mail.sender_idx = member_seq;
            mail.receiver_idx = Convert.ToInt32(reciever_seq);
            mail.sender = MemberModel.findByNo(mail.sender_idx);

            mail.Insert();


            MailBoxModel mail2 = MailBoxModel.findByTitle(mail.title, mail.content);



            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                MailBoxFileModel file = uploadedFiles[i];
                file.member_seq = Convert.ToInt32(reciever_seq);
                file.mailbox_idx = mail2.idx;
                file.Insert();
            }

            return Redirect("/mailbox/mailboxlist");
        }

        public IActionResult resend(int reciever_seq, int sender_seq, string title, string content, List<IFormFile> postedFiles)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<MailBoxFileModel> uploadedFiles = new List<MailBoxFileModel>();
            foreach (IFormFile postedFile in postedFiles)
            {
                MailBoxFileModel filemodel = new MailBoxFileModel();
                string fileName = Path.GetFileName(postedFile.FileName);
                filemodel.fileName = fileName;
                filemodel.fileUrl = path;

                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(filemodel);
                }
            }







            int member_seq = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            MailBoxModel mail = new MailBoxModel();
            mail.title = title;
            mail.content = content;
            mail.sender_idx = sender_seq;
            mail.receiver_idx = reciever_seq;
            mail.sender = MemberModel.findByNo(mail.sender_idx);

            mail.Insert();


            MailBoxModel mail2 = MailBoxModel.findByTitle(mail.title, mail.content);



            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                MailBoxFileModel file = uploadedFiles[i];
                file.member_seq = reciever_seq;
                file.mailbox_idx = mail2.idx;
                file.Insert();
            }

            return Redirect("/mailbox/mailboxlist");
        }


        public IActionResult detail(int idx)
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

            MailBoxModel mail = MailBoxModel.findByNo(idx);
            mail.checked_time = DateTime.Now;
            mail.Update(idx);

            List<MailBoxFileModel> files = MailBoxFileModel.FlieAll(member.member_seq, idx);




            ViewBag.files = files;




            mail.sender = MemberModel.findByNo(mail.sender_idx);


            ViewBag.mail = mail;
            ViewBag.member = member;

            return View();
        }

        public IActionResult delete(int idx)
        {

            MailBoxModel mail = MailBoxModel.findByNo(idx);
            mail.Delete(idx);



            return Redirect("/mailbox/mailboxlist");
        }



        public ActionResult DownloadFile(int idx, string fileName)
        {
            MailBoxFileModel file = MailBoxFileModel.findByFileName(idx, fileName);

            string fullName = file.fileUrl + "\\" + file.fileName;
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


