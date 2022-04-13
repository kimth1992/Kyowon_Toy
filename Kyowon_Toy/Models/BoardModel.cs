using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;

namespace Kyowon_Toy.Models
{
    public class BoardModel
    {
        public uint idx { get; set; }
        
        public string title { get; set; }
        public string contents { get; set; }
        public uint user { get; set; }
        public string userName { get; set; }
        public DateTime registeredDate { get; set; }

        public uint view_Cnt { get; set; }
        public short active { get; set; }
        public short type { get; set; }



        // 리미트 값 같은것을 넣어서 여기서 페이징 처리 해야되는 것 같아
        // 현재 이건 제목으로 검색한 걸 역순으로 하는 듯
        public static List<BoardModel> GetList(string search)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
idx, title, user, username, registeredDate, view_cnt, status_flag from board where
title like concat('%', IFNULL(@search,''), '%')
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
idx, title, user, username, registeredDate, view_cnt, active from board where active = 1";

                return db.Query<BoardModel>(sql, null);
            }
        }

        


        public static BoardModel Get(uint idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from board where idx = @idx and active = 1";
                return db.QuerySingle<BoardModel>(sql, new { idx = idx });
            }
        }

        public static BoardModel Next(uint idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from board where idx = (select min(idx) from board where idx > @idx and active = 1)";
                return db.QuerySingle<BoardModel>(sql, new { idx = idx });
            }
        }

        public static BoardModel Previous(uint idx)
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
now(), 0, 1, 1)";
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

        public int UpdateCount(uint idx)
        {
            string sql = @"update board set view_cnt = view_cnt + 1 where idx = @idx";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int Delete(uint idx)
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



