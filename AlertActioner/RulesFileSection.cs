using System;
using System.Configuration;

namespace AlertActioner
{
    public class RulesFileSection: ConfigurationSection
    {
        [ConfigurationProperty("Location", IsRequired = false)]
        public string Location => (string)this["Location"];
    }
}
