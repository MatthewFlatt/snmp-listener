using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonConfig;
using log4net;

namespace AlertActioner
{
    public class ConfigurationHandler
    {
        private static string _DefaultRuleFile = "ExampleRules.json";
        private static readonly ILog Logger = LogManager.GetLogger("Configuration");
        public static List<string> GetRulesFileLocation()
        {
            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (!appConfig.HasFile)
            {

                Logger.Warn($"Configuration file '{appConfig.FilePath}' not found, using default rules file - {_DefaultRuleFile}" );
                return new List<string> {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _DefaultRuleFile)};
            }
            var rulesFileSettingsSection = appConfig.GetSection("rules") as RulesFileSection;
            if (rulesFileSettingsSection == null ||  string.IsNullOrWhiteSpace(rulesFileSettingsSection.Location))
            {
                Logger.Warn($"Rules setting not found, using default rules file - {_DefaultRuleFile}");
                return new List<string> { Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _DefaultRuleFile)};
            }

            var locations = new List<string>();
            foreach (var location in rulesFileSettingsSection.Location.Split(','))
            {
                var trimmedLocation = location.Trim();
                // If we only have a filename, combine it with current dir
                locations.Add(string.IsNullOrWhiteSpace(Path.GetDirectoryName(trimmedLocation)) ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, trimmedLocation) : trimmedLocation);   
            }
            return locations;
        }

    }
}
