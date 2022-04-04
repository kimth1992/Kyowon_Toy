using KyowonToy.lib;
using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kyowon_Toy.Models
{
    public class MemberModel
    {
        public int Member_seq { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public DateTime BirthDay { get; set; }
        public int Grade { get; set; }

        public string Department { get; set; }
        public string Position { get; set; }
        public string Photo { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime WithdrawalDate { get; set; }
        public string Email { get; set; }
        public string Office_Tel { get; set; }
        public string Mobile_Tel { get; set; }
        public string Address { get; set; }
        public int Active { get; set; }



        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.password.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.password));

            this.password = System.Convert.ToBase64String(hash);
        }


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

        /*
        public int Insert()
        {
            string sql = "insert into member(name, sex) values(@name, @sex)";
            using (var db = new MysqlDapperHelper())
            {
                db.BeguinTransaction();
                try
                {
                    int r = 0;
                    r += db.Execute(sql, this);
                    r += db.Execute(sql, this);
                    r += db.Execute(sql, this);

                    db.Commit();

                    return r;

                }
                catch (Exception ex)
                {
                    db.RollBack();
                    throw ex;
                }
            }
        }
        */

        public int Insert()
        {
          
            string sql = @"
insert into member (
grade, 
name, 
password, 
birthDay,
mobile_tel, 
registeredDate,
active
)
values(
@grade,
@name, 
@password, 
@birthday,
@mobile_tel, 
now(),
1)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        internal MemberModel GetLoginUser(string name)
        {
            /*this.user_name
            this.password*/

            string sql = @"
select member_seq, name, password, department, position, registeredDate, email, mobile_tel
from member
where name = @name";

            MemberModel member;
            using (var db = new MysqlDapperHelper())
            {


                member = db.QuerySingle<MemberModel>(sql, this);
            }

            if (member == null)
            {
                throw new Exception("사용자가 존재하지 않습니다.");
            }

            if (member.password != this.password)
            {
                throw new Exception("비밀번호가 틀립니다!");
            }

            return member;


        }



    }
}
