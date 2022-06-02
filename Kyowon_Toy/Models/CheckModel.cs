using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class CheckModel
    {
        public int idx { get; set; }

        public int board_idx { get; set; } //  게시판 번호
        public int member_seq { get; set; } //  사람
        public DateTime registeredDate { get; set; } // 등록 시간


        public int Insert()
        {
            string sql = @"
insert into boardcheck (
board_idx,
member_seq,
registeredDate)
values(
@board_idx,
@member_seq,
now())";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        public static CheckModel findCheck(int idx, int member_seq)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
    select
     c.idx,
     c.registeredDate,
     b.idx,
     m.member_seq,
     m.name
    from
     boardcheck c
     inner join member m on c.member_seq=m.member_seq
     inner join board b on c.board_idx=b.idx
    where 
     c.board_idx=@idx and
     c.member_seq=@member_seq";

                return db.QuerySingle<CheckModel>(sql, new { idx = idx, member_seq = member_seq });
            }
        }

        public static List<CheckModel> CheckAll(int board_idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
SELECT idx, board_idx, member_seq, registeredDate 
from boardcheck 
WHERE board_idx = @board_idx
order by registeredDate desc";

                return db.Query<CheckModel>(sql, new { board_idx = board_idx });
            }
        }






    }

}



