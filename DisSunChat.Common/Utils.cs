using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisSunChat.Common
{
    public class Utils
    {
        public static void SaveLog(string s)
        {
            LoggerFunc c = LoggerFactory.CreateLoggerInstance();
            c.SaveErrorLogTxT(s);
        }
        public static string GetConfig(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                return "";
            }
        }

        public static T JsonToObject<T>(string jsonString)
        {            
            T t = JsonConvert.DeserializeObject<T>(jsonString);
            return t;
        }

        public static string ObjectToJsonStr(object obj)
        {
            string s = JsonConvert.SerializeObject(obj);
            return s;        
        }
    }
}
