using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertActioner
{
    /// <summary>
    /// The Severity of an Alert
    /// </summary>
    public enum Severity
    {
        // NOTE: The ordering of these severity values is important for ranking purposes.

        Unknown = -1,
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }
}
