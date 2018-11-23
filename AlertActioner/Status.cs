using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertActioner
{
    public enum Status
    {
        Unknown = -1,
        Ended = 0,
        Raised = 1,
        Escalated = 2,
        Cleared = 3
    }

}
