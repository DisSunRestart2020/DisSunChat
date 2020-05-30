using Fleck;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace DisSunChat.Common
{
    /// <summary>
    /// 所有调用WebSocket的帮助类必须遵从的协议
    /// </summary>
    public interface IWebSocketHelper
    {
        /// <summary>
        /// websocket连通后触发事件
        /// </summary>
        event SwitchEventHandler WsOpenEventHandler;
        /// <summary>
        /// websocket连接关闭后触发事件
        /// </summary>
        event SwitchEventHandler WsCloseEventHandler;
        /// <summary>
        /// websocket监听到消息后触发事件
        /// </summary>
        event ListenEventHandler WsListenEventHandler;
        /// <summary>
        /// websocket响应处理事件
        /// </summary>
        event ResponseTextEventHandler WsResponseTextEventHandler;

        /// <summary>
        /// 聊天室在线人数
        /// </summary>
        int PlayerCount
        {
            get;
            set;
        }

        /// <summary>
        /// websocket初始化
        /// </summary>
        void WebSocketInit();

        /// <summary>
        /// 向全员发送信息
        /// </summary>
        /// <param name="wsocketMsg"></param>
        void SendMessageToAll(WebSocketMessage wsocketMsg);
        /// <summary>
        /// 向自己发送信息
        /// </summary>
        /// <param name="wsocketMsg"></param>
        void SendMessageToMe(WebSocketMessage wsocketMsg);
    }


    #region 通用委托声明
    /// <summary>
    /// socket开关处理委托
    /// </summary>
    /// <returns></returns> 
    public delegate void SwitchEventHandler(object sender, WebsocketEventArgs e);

    /// <summary>
    /// socket监听处理委托
    /// </summary>
    /// <param name="socketData"></param>
    /// <param name="clientFrom"></param>
    /// <returns></returns>
    public delegate void ListenEventHandler(object sender, WebsocketEventArgs e);

    /// <summary>
    /// 反馈客户端的文本处理委托
    /// </summary>
    /// <param name="socketData"></param>
    /// <param name="cIp"></param>
    /// <param name="cPort"></param>
    /// <param name="cGuid"></param>
    /// <returns></returns>
    public delegate void ResponseTextEventHandler(object sender, WebsocketEventArgs e);
    #endregion

}
