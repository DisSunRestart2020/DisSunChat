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
    }
}
