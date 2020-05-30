using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    /// <summary>
    /// 通用webSocket通信载体
    /// </summary>
    [Serializable]
    public class WebSocketMessage
    {
        /// <summary>
        /// webSocket通信载体初始化
        /// </summary>
        /// <param name="cIp"></param>
        /// <param name="cPort"></param>
        /// <param name="cGuidID"></param>
        /// <param name="clientData"></param>
        public WebSocketMessage(string cIp, string cPort, string cGuidID, ClientData clientData)
        {
            this.CIp = cIp;
            this.CPort = cPort;
            this.CGuidID = cGuidID;
            this.ClientData = clientData;
            this.ChatTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ;
        }
        public WebSocketMessage()
        { }
        /// <summary>
        /// 客户端IP
        /// </summary>
        public string CIp
        {
            get;set;
        }
        /// <summary>
        /// 客户端端口
        /// </summary>
        public string CPort
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端ID
        /// </summary>
        public string CGuidID
        {
            get;
            set;
        }
        /// <summary>
        /// 通信时间
        /// </summary>
        public string ChatTime
        {
            get;
            set;
        }
        /// <summary>
        /// 客户发送的通信内容
        /// </summary>
        public ClientData ClientData
        {
            get;
            set;
        }
    }
}
