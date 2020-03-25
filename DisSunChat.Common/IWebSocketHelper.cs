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
        /// <summary>
        /// websocket连通后触发事件
        /// </summary>
        event SwitchHandle WsOpenEvent;
        /// <summary>
        /// websocket连接关闭后触发事件
        /// </summary>
        event SwitchHandle WsCloseEvent;
        /// <summary>
        /// websocket监听到消息后触发事件
        /// </summary>
        event ListenHandle ListenEvent;
        /// <summary>
        /// websocket响应处理事件
        /// </summary>
        event ResponseHandle ResponseEvent;

        /// <summary>
        /// websocket初始化
        /// </summary>
        void WebSocketInit();
        
        
    }


    /// <summary>
    /// socket开关处理委托
    /// </summary>
    /// <returns></returns>
    public delegate int SwitchHandle();
    /// <summary>
    /// socket监听处理委托
    /// </summary>
    /// <param name="socketData"></param>
    /// <param name="clientFrom"></param>
    /// <returns></returns>
    public delegate int ListenHandle(string socketData,string clientFrom);
    /// <summary>
    /// 响应文本处理委托
    /// </summary>
    /// <param name="socketData"></param>
    /// <param name="cIp"></param>
    /// <param name="cPort"></param>
    /// <param name="cGuid"></param>
    /// <returns></returns>
    public delegate string ResponseHandle(string socketData,string cIp, string cPort,string cGuid);
}
