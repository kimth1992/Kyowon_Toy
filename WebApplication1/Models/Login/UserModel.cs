using KyowonToy.lib.DataBase;
using MySqlConnector;
using System;

namespace WebApplication1.Models.Login
{
    public class UserModel
    {
        public uint user_seq { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }


        public void ConvertPassword()
        {
            var sha = new System.Security.Cryptography.HMACSHA512();
            sha.Key = System.Text.Encoding.UTF8.GetBytes(this.password.Length.ToString());

            var hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.password));

            this.password =  System.Convert.ToBase64String(hash);
        } 

        internal int Register()
        {

            string sql = @"
Insert into user(
user_name
,email
,password
)
values(
@user_name
,@email
,@password)";

            using (var db = new MysqlDapperHelper())
            {
               

                return db.Execute(sql, this);
            }
        }

        internal UserModel GetLoginUser()
        {
            /*this.user_name
            this.password*/

            string sql = @"
select user_seq, user_name, email, password
from user
where user_name = @user_name";

            UserModel user;
            using (var db = new MysqlDapperHelper())
            {
             

                user = db.QuerySingle<UserModel>(sql, this);
            }

            if(user == null)
            {
                throw new Exception("사용자가 존재하지 않습니다.");
            }

            if(user.password != this.password)
            {
                throw new Exception("비밀번호가 틀립니다!");
            }

            return user;


        }
    }
}
