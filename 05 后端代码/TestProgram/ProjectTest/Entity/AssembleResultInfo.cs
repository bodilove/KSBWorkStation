using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ProjectTest
{
    public class AssembleResultInfo
    {
        public int Id { get; set; }

        public int UUTestId { get; set; }

        public string PartSNNum { get; set; }

        public string Result { get; set; }

        public DateTime Scantime { get; set; }

        public int Type { get; set; }
    }
}
