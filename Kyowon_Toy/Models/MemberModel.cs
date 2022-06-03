using KyowonToy.lib;
using KyowonToy.lib.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Kyowon_Toy.Models
{
    public class MemberModel
    {
        public int member_seq { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public DateTime birthDay { get; set; }
        public int grade { get; set; } // 신입, 경력 구분
        public string department { get; set; }
        public string position { get; set; }
        public string photo { get; set; }

        public DateTime registeredDate { get; set; }
        public DateTime withdrawalDate { get; set; }
        public string email { get; set; }
        public string office_Tel { get; set; }
        public string mobile_Tel { get; set; }
        public string address { get; set; }
        public int active { get; set; }
        public string mainwork { get; set; }



        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.password.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.password));

            this.password = System.Convert.ToBase64String(hash);
        }

        public int Delete(int member_seq)
        {
            string sql = @"
update member set active = 0 where member_seq = @member_seq";

            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
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
active,
department,
position,
photo
)
values(
@grade,
@name, 
@password, 
@birthday,
@mobile_tel, 
now(),
1,
@department,
@position,
@photo)";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int UpdateEmail()
        {

            string sql = @"
Update member set email = @email where member_seq = @member_seq";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }


        public static List<MemberModel> mobileCheck(string mobile_tel)
        {

            string sql = @"
select member_seq, mobile_tel from member where mobile_tel = @mobile_tel";
            using (var db = new MysqlDapperHelper())
            {
                List<MemberModel> me = db.Query<MemberModel>(sql, new { mobile_tel = mobile_tel });

                int num = me.Count();

                return me;

            }
        }



        public int Update()
        {


            string sql = @"
Update member set
department = @department,
position = @position,
photo = @photo,
mobile_tel = @mobile_tel,
office_tel = @office_tel,
email = @email,
mainwork = @mainwork
where member_seq = @member_seq";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }

        public int UpdateAdmin()
        {


            string sql = @"
Update member set
member_seq = @member_seq,
name = @name,
department = @department,
position = @position,
office_tel = @office_tel,
mobile_tel = @mobile_tel,
email = @email
where member_seq = @member_seq";
            using (var db = new MysqlDapperHelper())
            {
                return db.Execute(sql, this);
            }
        }



        public static MemberModel Get(string name, string mobile_tel)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select member_seq, name, password, department, position, photo, email, office_tel, birthday,
mobile_tel, registeredDate, address, active from member where name = @name and mobile_tel = @mobile_tel";
                return db.QuerySingle<MemberModel>(sql, new { name = name , mobile_tel = mobile_tel });
            }
        }
        public static MemberModel findByNo(int member_seq)
        {
            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select member_seq, name, password, department, position, photo, email, office_tel, birthday,
mobile_tel, registeredDate, address, active, mainwork from member where member_seq = @member_seq";
                return db.QuerySingle<MemberModel>(sql, new { member_seq = member_seq });
            }
        }



        internal MemberModel GetLoginUser()
        {
            /*this.user_name
            this.password*/

            string sql = @"
select member_seq, name, password, department, position, registeredDate, email, mobile_tel
from member
where member_seq = @member_seq";

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
                // 비밀번호 틀린 횟수
            }

            return member;


        }


        public static List<MemberModel> SearchMember_seq(int member_seq)
        {

            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
member_seq, name, department, position, email, 
office_tel, mobile_tel, active
from member 
where
active = 1 and member_seq like concat('%', IFNULL(@member_seq,''), '%')
order by member_seq desc";



                return db.Query<MemberModel>(sql, new { member_seq = member_seq });
            }
        }

        public static List<MemberModel> SearchUsername(string name)
        {

            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
member_seq, name, department, position, email, 
office_tel, mobile_tel, active 
from member 
where
active = 1 and name like concat('%', IFNULL(@name,''), '%')
order by member_seq desc";



                return db.Query<MemberModel>(sql, new { name = name });
            }
        }

        public static List<MemberModel> SearchDepartment(string department)
        {

            using (var db = new MysqlDapperHelper())
            {
                string sql = @"
select
member_seq, name, department, position, email, 
office_tel, mobile_tel, active
from member 
where
active = 1 and department like concat('%', IFNULL(@department,''), '%')
order by member_seq desc";



                return db.Query<MemberModel>(sql, new { department = department });
            }
        }







    }
}
