using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DisSunChat.Common
{
    /// <summary>
    /// 日志类实体
    /// </summary>
    public class LoggerFunc
    {
        public FileInfo logCfg;
        public log4net.ILog errorLogger;
        public string QueueName;

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="title">日志内容</param>
        public void SaveErrorLogTxT(string title)
        {          
            errorLogger.Error(title);
        }        
    }
}
