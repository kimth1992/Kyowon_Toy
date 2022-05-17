using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class CommentModel
    {
        public int idx { get; set; }

        public int commentBoard_idx { get; set; }
        public int member_seq { get; set; }
        public string name { get; set; } // 댓글 작성자 이름
        public string content { get; set; }
        public DateTime registeredDate { get; set; }

        public short active { get; set; }



        public int Insert()
        {

            string sql = @"
insert into comment (
board_idx, 
member_seq, 
content, 
registeredDate,
active)
values(
@commentBoard_idx, 
@member_seq, 
@content, 
now(), 
1)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        public static List<CommentModel> CommentAll(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
SELECT c.idx, c.board_idx, c.member_seq, c.content, c.registeredDate, c.active, m.name 
from comment AS C
Left OUTER JOIN member as m
ON c.member_seq = m.member_seq
WHERE c.active = 1 AND c.board_idx = @idx";

                return db.Query<CommentModel>(sql, new { idx = idx });
            }
        }

        public int Delete(int idx)
        {

            string sql = @"
update comment set active = 0 where idx = @idx";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        public static CommentModel findByNo(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
SELECT
	c.idx,
	c.content,
	c.registeredDate,
	m.member_seq,
	m.name
FROM
	comment c
	INNER JOIN member m ON c.member_seq=m.member_seq
WHERE c.idx=@idx";

                return db.QuerySingle<CommentModel>(sql, new { idx = idx });
            }
        }




    }

}



