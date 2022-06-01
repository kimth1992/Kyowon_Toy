using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Kyowon_Toy.Models
{
    public class FileModel
    {
        public int idx { get; set; }

        public int board_idx { get; set; } 
        public string fileName { get; set; } 
        public string fileUrl { get; set; } 
      

        public int Insert()
        {

            string sql = @"
insert into file (
board_idx, 
fileName, 
fileUrl)
values(
@board_idx, 
@fileName, 
@fileUrl)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public static FileModel findByFileName(int idx, string fileName)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from file where fileName = @fileName and board_idx = @idx";
                return db.QuerySingle<FileModel>(sql, new { fileName = fileName , idx = idx});
            }
        }

        public static List<FileModel> FlieAll(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
SELECT f.idx, f.board_idx, f.filename, f.fileurl, b.registeredDate, b.active, b.title 
from file AS f
Left OUTER JOIN board as b
ON f.board_idx = b.idx
WHERE f.board_idx = @idx";

                return db.Query<FileModel>(sql, new { idx = idx });
            }
        }

        public static FileModel findByNo(int idx)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select * from file where board_idx = @idx";
                return db.QuerySingle<FileModel>(sql, new { idx = idx });
            }
        }

    }

}



