using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;

namespace Kyowon_Toy.Models
{
    public class BoardModel
    {
        public uint Idx { get; set; }
        
        public string Title { get; set; }
        public string Contents { get; set; }
        public uint User { get; set; }
        public string UserName { get; set; }
        public DateTime registeredDate { get; set; }

        public uint View_Cnt { get; set; }
        public short Status_Flag { get; set; }

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
idx, title, user, username, registeredDate, view_cnt, status_flag from board";

                return db.Query<BoardModel>(sql, null);
            }
        }



        public static BoardModel Get(uint idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select idx, title, contents, user, username, registeredDate, view_cnt from board where idx = @idx";
                return db.QuerySingle<BoardModel>(sql, new { idx = idx });
            }
        }

        void CheckContents()
        {
            if (string.IsNullOrWhiteSpace(this.Title))
            {
                throw new Exception("제목이 빈칸입니다.");
            }

            if (string.IsNullOrWhiteSpace(this.Contents))
            {
                throw new Exception("내용이 빈칸입니다.");
            }
            if (string.IsNullOrWhiteSpace(this.UserName))
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
status_flag
)
values(
@title, 
@contents, 
@user, 
@username, 
now(), 0, 0)";
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

        public int Delete()
        {
            string sql = @"
delete from board where idx = @idx";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

    }
}



