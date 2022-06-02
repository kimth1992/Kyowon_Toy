using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class BoardModel
    {
        public int idx { get; set; }
        
        public string title { get; set; }
        public string contents { get; set; }
        public int user { get; set; }
        public string userName { get; set; }
        public DateTime registeredDate { get; set; }

        public int view_Cnt { get; set; }
        public short active { get; set; }
        public int type { get; set; }
        public int like { get; set; }
        public string fileName { get; set; }


        public List<CommentModel> commentList { get; set; }
        public List<FileModel> fileList { get; set; }
        
       

        // 리미트 값 같은것을 넣어서 여기서 페이징 처리 해야되는 것 같아
        // 현재 이건 제목으로 검색한 걸 역순으로 하는 듯
        public static List<BoardModel> SearchTitle(string search)
        {
         
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, view_cnt, active from board where
title like concat('%', IFNULL(@search,''), '%')
order by idx desc";

              

                return db.Query<BoardModel>(sql, new {search = search }) ;
            }
        }

        public static List<BoardModel> SearchUsername(string search)
        {

            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, view_cnt, active from board where
username like concat('%', IFNULL(@search,''), '%')
order by idx desc";



                return db.Query<BoardModel>(sql, new { search = search });
            }
        }

        public static List<BoardModel> SearchDepartment(string search)
        {

            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
SELECT a.idx, a.title, a.user, a.username, a.registeredDate, a.view_cnt, a.active, b.department
from board AS a
left outer join member as b
on a.member_seq = b.member_seq
where
b.department like concat('%', IFNULL(@search,''), '%')
order by idx desc";



                return db.Query<BoardModel>(sql, new { search = search });
            }
        }

        /*  public static int GetCount()
          {
              using (var db = new MysqlDapperHelper())
              {
                  string sql = @"
  select count(idx) from board";

                  return db.Query<>(sql, null);
              }
          }*/

        public static List<BoardModel> BoardAll()
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, 
view_cnt, active, type
from board where active = 1";

                return db.Query<BoardModel>(sql, null);
            }
        }

        public static List<BoardModel> gBoardAll()
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, 
view_cnt, active, type
from board where active = 1 and type = 1";

                return db.Query<BoardModel>(sql, null);
            }
        }

        public static List<BoardModel> eBoardAll()
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, 
view_cnt, active, type
from board where active = 1 and type = 2";

                return db.Query<BoardModel>(sql, null);
            }
        }

        public static List<BoardModel> wBoardAll()
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, 
view_cnt, active, type
from board where active = 1 and type = 3";

                return db.Query<BoardModel>(sql, null);
            }
        }

        public static List<BoardModel> dBoardAll()
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, 
view_cnt, active, type
from board where active = 1 and type = 4";

                return db.Query<BoardModel>(sql, null);
            }
        }


        public static BoardModel Get(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from board where idx = @idx and active = 1";
                return db.QuerySingle<BoardModel>(sql, new { idx = idx });
            }
        }

        public static BoardModel findByTile(string title)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from board where title = @title";
                return db.QuerySingle<BoardModel>(sql, new { title = title });
            }
        }

        public static BoardModel Next(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from board where idx = (select min(idx) from board where idx > @idx and active = 1)";
                return db.QuerySingle<BoardModel>(sql, new { idx = idx });
            }
        }

        public static BoardModel Previous(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from board where idx = (select max(idx) from board where idx < @idx and active = 1)";
                return db.QuerySingle<BoardModel>(sql, new { idx = idx });
            }
        }


        void CheckContents()
        {
            if (string.IsNullOrWhiteSpace(this.title))
            {
                throw new Exception("제목이 빈칸입니다.");
            }

            if (string.IsNullOrWhiteSpace(this.contents))
            {
                throw new Exception("내용이 빈칸입니다.");
            }
            if (string.IsNullOrWhiteSpace(this.userName))
            {
                throw new Exception("작성자가 없습니다.");
            }
        }

        public int Insert()
        {
            CheckContents();
            string sql = @"
insert into board (
title, 
contents, 
user, 
username, 
registeredDate, 
view_cnt, 
active,
type
)
values(
@title, 
@contents, 
@user, 
@username, 
now(), 0, 1, @type)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int Update()
        {
            CheckContents();
            string sql = @"
Update board set title = @title, contents = @contents where idx = @idx";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int UpdateCount(int idx)
        {
            string sql = @"update board set view_cnt = view_cnt + 1 where idx = @idx";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int Delete(int idx)
        {
            string sql = @"
update board set active = 0 where idx = @idx";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

    }
}



