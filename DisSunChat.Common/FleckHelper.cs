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
    public class FleckHelper:IWebSocketHelper
    {
        public event SwitchHandle WsOpenEvent;
        public event SwitchHandle WsCloseEvent;
        public event ListenHandle ListenEvent;
        public event ResponseTextHandle ResponseTextEvent;
        /// <summary>
        /// 聊天室在线人数
        /// </summary>
        public int PlayerCount
        {
            get;
            set;
        }
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
                    //自定义处理
                    Utils.SaveLog("WebSocket已经开启");
                    if (this.WsOpenEvent != null)
                    {
                        this.WsOpenEvent();
                    }
                };

                socket.OnClose = () => {
                    //从连接集合中移除
                    Utils.SaveLog("WebSocket已经关闭");
                    for (int i= socketListHs.Count-1; i>=0;i--)
                    {
                        if (socketListHs[i] == null)
                        {                           
                            socketListHs.Remove(i);
                        }                        
                    }
                    PlayerCount = socketListHs.Count;
                    //自定义处理
                    if (this.WsCloseEvent != null)
                    {
                        this.WsCloseEvent();
                    }
                };

                socket.OnMessage = (message) =>
                {

                    ClientData cData = Utils.JsonToObject<ClientData>(message);
                    WebSocketMessage wsocketMsg = new WebSocketMessage(socket.ConnectionInfo.ClientIpAddress, socket.ConnectionInfo.ClientPort.ToString(), socket.ConnectionInfo.Id.ToString("N"), cData);

                    if (Convert.ToBoolean(cData.IsConnSign))
                    {
                        if (!socketListHs.ContainsKey(cData.IdentityMd5))
                        {
                            socketListHs.Add(cData.IdentityMd5, socket);
                        }
                        else
                        {
                            socketListHs[cData.IdentityMd5] = socket;
                        }
                        PlayerCount = socketListHs.Count;
                    }

                    if (this.ListenEvent != null)
                    {
                        this.ListenEvent(wsocketMsg);
                    }
                };

            });
        }   

        /// <summary>
        /// 向全员发送信息
        /// </summary>
        /// <param name="wsocketMsg"></param>
        public void SendMessageToAll(WebSocketMessage wsocketMsg)
        {           
            string resultData = "";
            if (this.ResponseTextEvent != null)
            {
                resultData = this.ResponseTextEvent(wsocketMsg);
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

        /// <summary>
        /// 向自己发送信息
        /// </summary>
        /// <param name="wsocketMsg"></param>
        public void SendMessageToMe(WebSocketMessage wsocketMsg)
        {
            string resultData = "";
            if (this.ResponseTextEvent != null)
            {
                resultData = this.ResponseTextEvent(wsocketMsg);
            }

            if (!string.IsNullOrWhiteSpace(resultData))
            {
                foreach (DictionaryEntry dey in socketListHs)
                {
                    IWebSocketConnection subConn = (IWebSocketConnection)dey.Value;
                    if(subConn.ConnectionInfo.Id.ToString("N")== wsocketMsg.CGuidID)
                    {
                        subConn.Send(resultData);
                        break;
                    }                    
                }
            }
        }
    }


}
