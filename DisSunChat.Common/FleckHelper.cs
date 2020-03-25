using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
        private Hashtable socketListHs = new Hashtable();
      
        public void WebSocketInit()
        {
            string websocketPath = Utils.GetConfig("websocketPath");
            WebSocketServer wsServer = new WebSocketServer(websocketPath);
            

            wsServer.Start(socket =>
            {
                socket.OnOpen = () => {                    
                    SocketOpen();
                };

                socket.OnClose = () => {

                    for(int i= socketListHs.Count-1; i>=0;i--)
                    {
                        if (socketListHs[i] == null)
                        {                           
                            socketListHs.Remove(i);
                        }                        
                    }                 
                    SocketClose();
                };

                socket.OnMessage = (message) =>
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(message);
                    string identityMd5 = jo["identityMd5"].ToString();
                    string sMsg = jo["sMsg"].ToString();
                    bool isOpenLink = Convert.ToBoolean(jo["isOpenLink"]);

                    if (isOpenLink)
                    {
                        if (!socketListHs.ContainsKey(identityMd5))
                        {
                            socketListHs.Add(identityMd5, socket);
                        }
                        else
                        {
                            socketListHs[identityMd5] = socket;
                        }
                    }
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
            JObject jo = (JObject)JsonConvert.DeserializeObject(socketData);
            string identityMd5 = jo["identityMd5"].ToString();
            string sMsg = jo["sMsg"].ToString();
            bool isOpenLink = Convert.ToBoolean(jo["isOpenLink"]);

            IWebSocketConnection socketConn = (IWebSocketConnection)socketListHs[identityMd5];
            string cAddress= socketConn.ConnectionInfo.ClientIpAddress;
            string cPort= socketConn.ConnectionInfo.ClientPort.ToString();
            string clientFrom= cAddress + ":" + cPort;

            if (isOpenLink)
            {
                string newsMsg = clientFrom+"加入了群聊";
                string searchStr = "\"sMsg\":\"";
                int position = socketData.IndexOf(searchStr);
                socketData = socketData.Insert(position + searchStr.Length, newsMsg);
                //立刻反馈
                SendMessage(socketData);
            }
            else
            {
                if (this.ListenEvent != null)
                {
                    this.ListenEvent(socketData, clientFrom);
                }
                //立刻反馈
                SendMessage(socketData);
            }
        }

        public void SendMessage(string socketData)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(socketData);
            string identityMd5 = jo["identityMd5"].ToString();
            string sMsg = jo["sMsg"].ToString();
            IWebSocketConnection socketConn = (IWebSocketConnection)socketListHs[identityMd5];

            string cIp = socketConn.ConnectionInfo.ClientIpAddress;
            string cPort = socketConn.ConnectionInfo.ClientPort.ToString();
            string cGuid = socketConn.ConnectionInfo.Id.ToString("N");

            string resultData = "";
            if (this.ResponseEvent != null)
            {
                resultData = this.ResponseEvent(socketData, cIp, cPort, cGuid);
            }

            if (!string.IsNullOrWhiteSpace(resultData))
            {
                foreach (DictionaryEntry dey in socketListHs)
                {
                    IWebSocketConnection subConn = (IWebSocketConnection)dey.Value;
                    subConn.Send(resultData);
                }          
            }


        }
    }


}
