using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Common
{
    public class User
    {
        public enum enumAuthority
        {
            RunTest,
            EditTest,
            UserManage,
            Maintain
        }

        public string Name;
        public string Description;
        public User.enumAuthority Authority;

    }
}
