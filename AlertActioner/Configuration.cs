using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonConfig;

namespace AlertActioner
{
    public class Configuration
    {
        public int GetConfigVersion()
        {
            return Config.Default.Version;
        }

        public dynamic GetRuleSet()
        {
            return Config.User.Rules;
        }
    }
}
