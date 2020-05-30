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
        /// <summary>
        /// websocket连通后触发事件
        /// </summary>
        public event SwitchEventHandler WsOpenEventHandler;
        /// <summary>
        /// websocket连接关闭后触发事件
        /// </summary>
        public event SwitchEventHandler WsCloseEventHandler;
        /// <summary>
        /// websocket监听到消息后触发事件
        /// </summary>
        public event ListenEventHandler WsListenEventHandler;
        /// <summary>
        /// websocket反馈客户端的文本处理事件
        /// </summary>
        public event ResponseTextEventHandler WsResponseTextEventHandler;       
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
                //以下的设置，每当一个新连接进来，都会生效。
                socket.OnOpen = () => {
                    //自定义处理
                    
                    if (this.WsOpenEventHandler != null)
                    {
                        WebsocketEventArgs args = new WebsocketEventArgs();
                        this.WsOpenEventHandler(this, args);
                    }
                };

                socket.OnClose = () => {
                    //从连接集合中移除                    
                    for (int i= socketListHs.Count-1; i>=0;i--)
                    {
                        if (socketListHs[i] == null)
                        {                           
                            socketListHs.Remove(i);
                        }                        
                    }
                    PlayerCount = socketListHs.Count;
                    //自定义处理
                    if (this.WsCloseEventHandler != null)
                    {
                        WebsocketEventArgs args = new WebsocketEventArgs();
                        this.WsCloseEventHandler(this, args);
                    }
                };

                socket.OnMessage = (message) =>
                {
                    ClientData cData = Utils.JsonToObject<ClientData>(message);
                    WebSocketMessage wsocketMsg = new WebSocketMessage(socket.ConnectionInfo.ClientIpAddress, socket.ConnectionInfo.ClientPort.ToString(), socket.ConnectionInfo.Id.ToString("N"), cData);

                    if (Convert.ToBoolean(cData.IsConnSign))
                    {
                        //收到用户上线信息，更新socket列表
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

                    if (this.WsListenEventHandler != null)
                    {
                        WebsocketEventArgs args = new WebsocketEventArgs();
                        args.WebSocketMessage = wsocketMsg;
                        this.WsListenEventHandler(this, args);
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
            if (this.WsResponseTextEventHandler != null)
            {
                WebsocketEventArgs args = new WebsocketEventArgs();
                args.WebSocketMessage = wsocketMsg;
                this.WsResponseTextEventHandler(this, args);
                resultData = args.ResultDataMsg;
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
            if (this.WsResponseTextEventHandler != null)
            {
                WebsocketEventArgs args = new WebsocketEventArgs();
                args.WebSocketMessage = wsocketMsg;
                this.WsResponseTextEventHandler(this, args);
                resultData = args.ResultDataMsg;                 
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
