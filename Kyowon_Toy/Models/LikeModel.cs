using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class LikeModel
    {
        public int idx { get; set; }

        public int board_idx { get; set; } // 좋아요 달린 게시판 번호
        public int member_seq { get; set; } // 좋아요 누른 사람
        public int likeOrNot { get; set; } // 좋아요 여부
      

        public int Insert()
        {

            string sql = @"
insert into boardlike (
board_idx, 
member_seq, 
likeOrNot)
values(
@board_idx, 
@member_seq, 
1)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        // 게시판별 좋아요 정보(게시판 번호로 조회)
        public static List<LikeModel> findBoardCount(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
 select
     l.idx,
     l.likeOrNot,
     b.idx,
     m.member_seq,
     m.name
    from
     boardlike l
     inner join member m on l.member_seq=m.member_seq
     inner join board b on l.board_idx=b.idx
    where 
     l.board_idx=@idx";

                return db.Query<LikeModel>(sql, new { idx = idx });
            }
        }

        // 게시판에 좋아요 눌렀는지 안눌렀는지(게시판 번호와 멤버 번호를 가지고 조회)
        public static LikeModel findBoardLike(int idx, int member_seq)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
    select
     l.idx,
     l.likeOrNot,
     b.idx,
     m.member_seq,
     m.name
    from
     boardlike l
     inner join member m on l.member_seq=m.member_seq
     inner join board b on l.board_idx=b.idx
    where 
     l.board_idx=@idx and
     l.member_seq=@member_seq";

                return db.QuerySingle<LikeModel>(sql, new { idx = idx, member_seq = member_seq });
            }
        }





        public int Delete()
        {

            string sql = @"
delete from boardlike 
where board_idx=@board_idx and 
member_seq=@member_seq";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

              

    }

}



