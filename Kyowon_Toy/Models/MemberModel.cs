using MySqlConnector;
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
            using (var conn = new MySqlConnection("Server = 127.0.0.1; Port = 3306; Database = kyowontoy; Uid = root; Pwd = root;"))
            {

                conn.Open();

                string sql = @"select m.no
, m.name
,m.sex
FROM
member m
where
m.sex = @sex";

                return Dapper.SqlMapper.Query<MemberModel>(conn, sql, new { sex = sex }).ToList();

            }
        }



        public int Insert()
        {
            string sql = "insert into member(name, sex) values(@name, @sex)";
            using (var conn = new MySqlConnection("Server = 127.0.0.1; Port = 3306; Database = kyowontoy; Uid = root; Pwd = root;"))
            {

                conn.Open();

                return Dapper.SqlMapper.Execute(conn, sql, this);
            }
        }



    }
}
