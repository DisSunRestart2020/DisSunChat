using Fleck;
using System;
using System.Collections.Generic;
using System.Text;

namespace DisSunChat.Common
{
    /// <summary>
    /// Fleck 帮助类，实现webSocket的调用
    /// </summary>
    public  class FleckHelper:IWebSocketHelper
    {
        private IWebSocketConnection connSocket;
        public void WebSocketInit()
        {
            WebSocketServer wsServer = new WebSocketServer("ws://172.16.2.4:8181");
            wsServer.Start(socket =>
            {
                connSocket = socket;

                socket.OnOpen = () => {
                    SocketOpen();
                };

                socket.OnClose = () => {
                    SocketClose();
                };

                socket.OnMessage = (message) =>{
                    ListenMessage(message);
                };

            });
        }

        public void SocketOpen()
        {
            Utils.SaveLog("成功建立长连接！");
        }

        public void SocketClose() 
        {
            Utils.SaveLog("关闭长连接");
        }


        public void ListenMessage(string requestMsg)
        {
            
            string cId = connSocket.ConnectionInfo.Id.ToString("N");
            string cAddress= connSocket.ConnectionInfo.ClientIpAddress;
            string cPort= connSocket.ConnectionInfo.ClientPort.ToString();
            string clientUrl = "["+ cId + "]"+ cAddress + ":" + cPort;
            Utils.SaveLog("接收到客户端=" + clientUrl+"，具体信息="+ requestMsg);
            //立刻反馈
            SendMessage(requestMsg);
        }

        public void SendMessage(string requestMsg)
        {           
            string respondStr = "{\"ClientName\":\"172.16.2.4:00\",\"CreateTime\":\"2020-03-03 12:45:24\",\"ChatContent\":\"这里是内容\",\"PrevMsg\":\"" + requestMsg +"\"}";
            connSocket.Send(respondStr);
            
        }

    }
}
