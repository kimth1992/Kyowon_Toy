using System;

namespace WebApplication1.Models

{
    public class Member
    {
        private int member_seq;
        private string name;
        private string password;
        private string department;
        private string position; // 직책 : 본부장, 부분장, 팀장, 파트장 매니저 등
        private string photo;
        private DateTime registeredDate;
        private DateTime withdrawalDate;
        private int active; // 활성, 비활성화
        private string email;
        private string office_tel;
        private string mobile_tel;
        private string address;
     //   private int grade; 접근권한 설정 시 사용 예정
        

        public int Member_seq
        {
            get { return member_seq; }
            set { member_seq = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Department
        {
            get { return department; }
            set { department = value; }
        }
        public string Position
        {
            get { return position; }
            set { position = value; }
        }
        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }
        public DateTime RegisteredDate
        {
            get { return registeredDate; }
            set { registeredDate = value; }
        }
        public DateTime WithdrawalDate
        {
            get { return withdrawalDate; }
            set { withdrawalDate = value; }
        }
        public int Active
        {
            get { return active; }
            set { active = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Office_tel
        {
            get { return office_tel; }
            set { office_tel = value; }
        }
        public string Mobile_tel
        {
            get { return mobile_tel; }
            set { mobile_tel = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
 
  
    }
}
