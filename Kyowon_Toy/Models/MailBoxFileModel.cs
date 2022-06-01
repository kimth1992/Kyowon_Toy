using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class MailBoxFileModel
    {
        public int idx { get; set; }

        public int member_seq { get; set; } 
        public string fileName { get; set; } 
        public string fileUrl { get; set; } 
        public int mailbox_idx { get; set; }
      

        public int Insert()
        {

            string sql = @"
insert into mailboxFile (
member_seq, 
fileName, 
fileUrl,
mailbox_idx)
values(
@member_seq, 
@fileName, 
@fileUrl,
@mailbox_idx)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public static MailBoxFileModel findByFileName(int idx, string fileName)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from mailboxFile where fileName = @fileName and member_seq = @idx";
                return db.QuerySingle<MailBoxFileModel>(sql, new { fileName = fileName , idx = idx});
            }
        }

        public static List<MailBoxFileModel> FlieAll(int idx, int mailbox_idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
SELECT f.idx, f.member_seq, f.filename, f.fileurl, b.name, b.active, b.email
from mailboxFile AS f
Left OUTER JOIN member as b
ON f.member_seq = b.member_seq
WHERE f.member_seq = @idx and f.mailbox_idx = @mailbox_idx";

                return db.Query<MailBoxFileModel>(sql, new { idx = idx, mailbox_idx });
            }
        }

        public static MailBoxFileModel findByNo(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from mailboxFile where member_seq = @idx";
                return db.QuerySingle<MailBoxFileModel>(sql, new { idx = idx });
            }
        }

    }

}



