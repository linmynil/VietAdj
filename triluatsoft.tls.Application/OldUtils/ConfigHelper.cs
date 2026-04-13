using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace triluatsoft.tls.OldUtils
{
    public class ConfigHelper
    {
        /// <summary>
        /// Gets value in AppSettings from web.config file
        /// </summary>
        /// <param name="configKey">The config key (Ex: ConfigKeys.KEY)</param>
        /// <param name="defaultValue">The default value to be returned if the key not found or not set in web.config file</param>
        /// <returns></returns>
        public static int GetIntOrDefault(string configKey, int defaultValue = 0)
        {
            int value;
            if (!int.TryParse(ConfigurationManager.AppSettings[configKey], out value))
                value = defaultValue;
            return value;
        }

        /// <summary>
        /// Gets value in AppSettings from web.config file
        /// </summary>
        /// <param name="configKey">The config key (Ex: ConfigKeys.KEY)</param>
        /// <param name="defaultValue">The default value to be returned if the key not found or not set in web.config file</param>
        /// <returns></returns>
        public static string GetString(string configKey, string defaultValue = "")
        {
            string value = ConfigurationManager.AppSettings[configKey];
            if (string.IsNullOrWhiteSpace(value))
                value = defaultValue;
            return value;
        }
    }
}
