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

        /// <summary>
        /// websocket已经连通的连接集合
        /// </summary>
        private Hashtable socketListHs = new Hashtable();
      
        public void WebSocketInit()
        {
            string websocketPath = Utils.GetConfig("websocketPath");
            WebSocketServer wsServer = new WebSocketServer(websocketPath);
            

            wsServer.Start(socket =>
            {         
                socket.OnOpen = () => {
                    Utils.SaveLog("成功建立长连接！"+socket.ConnectionInfo.Id.ToString("N"));
                    //自定义处理
                    if (this.WsOpenEvent != null)
                    {
                        this.WsOpenEvent();
                    }
                };

                socket.OnClose = () => {
                    Utils.SaveLog("断开一个长连接");

                    //从连接集合中移除
                    for (int i= socketListHs.Count-1; i>=0;i--)
                    {
                        if (socketListHs[i] == null)
                        {                           
                            socketListHs.Remove(i);
                        }                        
                    }

                    //自定义处理
                    if (this.WsCloseEvent != null)
                    {
                        this.WsCloseEvent();
                    }
                };

                socket.OnMessage = (message) =>
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(message);
                    string identityMd5 = jo["identityMd5"].ToString();
                    string sMsg = jo["sMsg"].ToString();
                    bool isConnSign = Convert.ToBoolean(jo["isConnSign"]);

                    if (isConnSign)
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


        public void ListenMessage(string socketData)
        {
            JObject jo = (JObject)JsonConvert.DeserializeObject(socketData);
            string identityMd5 = jo["identityMd5"].ToString();
            string sMsg = jo["sMsg"].ToString();
            bool isConnSign = Convert.ToBoolean(jo["isConnSign"]);

            IWebSocketConnection socketConn = (IWebSocketConnection)socketListHs[identityMd5];
            string cAddress= socketConn.ConnectionInfo.ClientIpAddress;
            string cPort= socketConn.ConnectionInfo.ClientPort.ToString();
            string clientFrom= cAddress + ":" + cPort;

            if (isConnSign)
            {
                string newsMsg = clientFrom+"加入了群聊(共"+ socketListHs.Count + "人在线)";
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
