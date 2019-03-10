using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    class Helper
    {
        public static string GetConfiguration(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                LogHelper.Log("CONFIGURATION", ex.Message);
                return "";
            }
        }

        public static string GetConnectionString(string name)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
            catch (Exception ex)
            {
                LogHelper.Log("CONNECTION_STRING", ex.Message);
                return "";
            }
        }
    }
}
