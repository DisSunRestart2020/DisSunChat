using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DisSunChat.Common
{
    /// <summary>
    /// 日志单例工厂
    /// </summary>
    public class LoggerFactory
    {
        public static string CommonQueueName = "DisSunQueue";
        private static LoggerFunc log2;
        private static object logKey = new object();
        public static LoggerFunc CreateLoggerInstance()
        {
            if (log2 != null)
            {
                return log2;
            }

            lock (logKey)
            {
                if (log2 == null)
                {
                    log4net.Repository.ILoggerRepository repository = log4net.LogManager.CreateRepository(CommonQueueName);
                    string log4NetPath = AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
                    log2 = new LoggerFunc();
                    log2.logCfg = new FileInfo(log4NetPath);                
                    log4net.Config.XmlConfigurator.Configure(repository, log2.logCfg);
                    log4net.Config.BasicConfigurator.Configure(repository);
                    log2.errorLogger = log4net.LogManager.GetLogger(repository.Name, "MyError");
                }
            }

            return log2;
        }
    }
}
