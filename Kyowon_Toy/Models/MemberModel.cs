using KyowonToy.lib;
using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyowon_Toy.Models
{
    public class MemberModel
    {
        public int No { get; set; }
        public string Name { get; set; }   
        public string Sex { get; set; }

        public static List<MemberModel> GetList(string sex)
        {
       
            using (var db = new MysqlDapperHelper())
            {

               
                string sql = @"select m.no
, m.name
,m.sex
FROM
member m
where
m.sex = @sex";

                return db.Query<MemberModel>(sql, new { sex = sex });

            }
        }



        public int Insert()
        {
            string sql = "insert into member(name, sex) values(@name, @sex)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }



    }
}
