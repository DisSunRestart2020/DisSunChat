using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolGood.Words;

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


        /// <summary>
        /// 敏感词过滤
        /// </summary>
        /// <param name="sourceWord"></param>
        /// <returns></returns>
        public static string ReplaceIllegalWord(string sourceWord)
        {         
            string path = AppDomain.CurrentDomain.BaseDirectory + "JsCommon\\illegalWord.txt";
            StreamReader illegalStreamReader = new StreamReader(path);            
            List<string> wordList = new List<string>();           
            string line = "";
            while ((line = illegalStreamReader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                wordList.Add(line.Trim());
            }
            illegalStreamReader.Close();
            illegalStreamReader.Dispose();
            StringSearch iwords = new StringSearch();
            iwords.SetKeywords(wordList.ToArray());
            return iwords.Replace(sourceWord, '*');
        }

    }
}
