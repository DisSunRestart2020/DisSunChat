using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    public class WebsocketEventArgs : EventArgs
    {
        /// <summary>
        /// 通用webSocket通信载体
        /// </summary>
        public WebSocketMessage WebSocketMessage
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string ResultDataMsg
        {
            get;
            set;
        }
    }
}
