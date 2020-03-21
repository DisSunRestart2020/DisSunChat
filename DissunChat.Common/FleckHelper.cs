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
        public event SwitchHandle WsOpenEvent;
        public event SwitchHandle WsCloseEvent;
        public event ListenHandle ListenEvent;
        public event ResponseHandle ResponseEvent;

        private IWebSocketConnection connSocket;         
        public void WebSocketInit()
        {
            string websocketPath = Utils.GetConfig("websocketPath");
            WebSocketServer wsServer = new WebSocketServer(websocketPath);
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
            //Utils.SaveLog("成功建立长连接！");
            if(this.WsOpenEvent!=null)
            {
                this.WsOpenEvent();
            }
        }

        public void SocketClose() 
        {
            //Utils.SaveLog("关闭长连接");
            if (this.WsCloseEvent != null)
            {
                this.WsCloseEvent();
            }
        }

        public void ListenMessage(string socketData)
        {
          
            string cAddress= connSocket.ConnectionInfo.ClientIpAddress;
            string cPort= connSocket.ConnectionInfo.ClientPort.ToString();
            string clientFrom= cAddress + ":" + cPort;

            if (this.ListenEvent != null)
            {
                this.ListenEvent(socketData, clientFrom);
            }

            //立刻反馈
            SendMessage(socketData);
        }

        public void SendMessage(string socketData)
        {
            //string respondStr = "{\"ClientName\":\"172.16.2.4:00\",\"CreateTime\":\"2020-03-03 12:45:24\",\"ChatContent\":\"这里是内容\",\"PrevMsg\":\"" + requestMsg +"\"}";
            string cAddress = connSocket.ConnectionInfo.ClientIpAddress;
            string cPort = connSocket.ConnectionInfo.ClientPort.ToString();
            string clientFrom = cAddress + ":" + cPort;

            string resultData = "";
            if (this.ResponseEvent != null)
            {
                resultData = this.ResponseEvent(socketData, clientFrom);
            }

            if (!string.IsNullOrWhiteSpace(resultData))
            { 
                connSocket.Send(resultData);
            }                   
        }
    }


}
