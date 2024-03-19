using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartUp.Enity
{
    public  class GroupsEnity
    {
        public string Type { get; set; }

        public int Test { get; set; }

        public bool Test_Bool { get
            {
                switch (Test)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public int Build { get; set; }

        public bool Build_Bool
        {
            get
            {
                switch (Build)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public int Manage { get; set; }

        public bool Manage_Bool
        {
            get
            {
                switch (Manage)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public int Query { get; set; }

        public bool Query_Bool
        {
            get
            {
                switch (Query)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public int SetLabel { get; set; }

        public bool SetLabel_Bool
        {
            get
            {
                switch (SetLabel)
                {
                    case 0:
                        return false;
                    case 1:
                        return true;

                    default:
                        return false;
                }
            }
        }

    }
    public class GroupsIntEnity
    {
        public int id { get; set; }

        public string Type { get; set; }

        public int Test { get; set; }
        public int Build { get; set; }
        public int Manage { get; set; }

        public int Query { get; set; }

        public int SetLabel { get; set; }
    }
}
