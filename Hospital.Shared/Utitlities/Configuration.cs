using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Utitlities
{
    /// <summary>
    /// Configuration
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// ConfigurationManager
        /// </summary>
        private static ConfigurationManager configurationManager;
        /// <summary>
        /// configurationManager1
        /// </summary>
        /// <param name="configurationManager1"></param>
        public static void Configure(ConfigurationManager configurationManager1)
        {
            configurationManager = configurationManager1;
        }

        public static ConfigurationManager GetConfigurationManager()
        {
            return configurationManager;
        }

    }
}
