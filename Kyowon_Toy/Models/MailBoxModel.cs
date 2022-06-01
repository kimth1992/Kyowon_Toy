using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class MailBoxModel
    {
        public int idx { get; set; }

        public int sender_idx { get; set; } 
        public int receiver_idx { get; set; } 
        public string title { get; set; } 
        public string content { get; set; } 
        public DateTime sent_time { get; set; }
        public DateTime checked_time { get; set; }
        public MemberModel sender { get; set; }
        public int active { get; set; }




        public int Insert()
        {

            string sql = @"
insert into mailbox (
sender_idx, 
receiver_idx, 
title,
content,
active)
values(
@sender_idx, 
@receiver_idx,
@title,
@content,
1)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        // 게시판별 좋아요 정보(게시판 번호로 조회)
        public static List<MailBoxModel> findByAll()
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
 select
       x.idx,
       x.title,
       x.content,
       x.sent_time,
       x.checked_time,
       x.active,
       m.member_seq sender_idx,
       m.name sender_name,
       m.email sender_email,
       m2.member_seq receiver_idx,
       m2.name receiver_name,
       m2.email receiver_email
    from
      mailbox x
      inner join member m on x.sender_idx=m.member_seq
      inner join member m2 on x.receiver_idx=m2.member_seq
   where
   	x.active = 1
    order by
      x.sent_time DESC";

              //  return db.Query<MailBoxModel>(sql, new { member_seq = member_seq });
                return db.Query<MailBoxModel>(sql, null);

            }
        }

       
        public static MailBoxModel findByNo(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
    select
     idx,
     sender_idx,
     receiver_idx,
     title,
     content,
     sent_time,
     checked_time,
     active
    from
     mailbox 
    where 
     idx = @idx and active = 1";

                return db.QuerySingle<MailBoxModel>(sql, new { idx = idx});
            }
        }

        public static MailBoxModel findByTitle(string title, string content)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
    select
     idx,
     sender_idx,
     receiver_idx,
     title,
     content,
     sent_time,
     checked_time,
     active
    from
     mailbox 
    where 
     title = @title and content = @content and active = 1";

                return db.QuerySingle<MailBoxModel>(sql, new{title = title, content = content});
            }
        }



        public int Delete(int idx)
        {

            string sql = @"
update mailbox set
active = 0
where
idx = @idx";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, new { idx = idx });
            }
        }

        public int Update(int idx)
        {

            string sql = @"
update mailbox set
checked_time=now()
where
idx = @idx and active = 1";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, new { idx = idx });
            }
        }


    }

}



