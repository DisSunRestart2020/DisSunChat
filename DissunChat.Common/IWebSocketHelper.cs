using Fleck;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisSunChat.Common
{
    /// <summary>
    /// 所有调用WebSocket的帮助类必须遵从的协议
    /// </summary>
    public interface IWebSocketHelper
    {
        void WebSocketInit();
        void SocketOpen();

        void SocketClose();

        void ListenMessage(string requestMsg);

        void SendMessage(string requestMsg);
        
            
    }
}
