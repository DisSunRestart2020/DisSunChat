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
        event SwitchHandle WsOpenEvent;
        event SwitchHandle WsCloseEvent;
        event ListenHandle ListenEvent;
        event ResponseHandle ResponseEvent;


        void WebSocketInit();
        void SocketOpen();

        void SocketClose();

        void ListenMessage(string requestMsg);

        void SendMessage(string requestMsg);      
    }

    public delegate int SwitchHandle();
    public delegate int ListenHandle(string socketData,string clientFrom);
    public delegate string ResponseHandle(string socketData,string cIp, string cPort,string cGuid);
}
