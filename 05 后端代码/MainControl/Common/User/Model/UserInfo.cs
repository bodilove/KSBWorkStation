using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;

namespace Common.User.Model
{
    [DataContract]
    public class UserInfo
    {
        private int userId;
        [DataMember]
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        private string userNum;
        /// <summary>
        /// 工号
        /// </summary>
        [DataMember]
        public string UserNum
        {
            get { return userNum; }
            set { userNum = value; }
        }
        private string userName;
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        private string passWord;
        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        private int age;
        /// <summary>
        /// 年龄
        /// </summary>
        [DataMember]
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        private int sex;
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember]
        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        private int jurisdictionId;
        /// <summary>
        /// 权限设置
        /// </summary>
        [DataMember]
        public int JurisdictionId
        {
            get { return jurisdictionId; }
            set { jurisdictionId = value; }
        }




    }
}
