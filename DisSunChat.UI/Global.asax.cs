
using DisSunChat.Common;
using DisSunChat.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DisSunChat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ChatService chatService = new ChatService();

            IWebSocketHelper helper = new FleckHelper();
            helper.WsOpenEvent += () => {
                //Utils.SaveLog("WebSocket已经开启");
                return 1;
            };

            helper.WsCloseEvent += () => {
                //Utils.SaveLog("WebSocket已经关闭");
                return 1;
            };

            helper.ListenEvent += (wsocketMsg) =>
            {
                if (!Convert.ToBoolean(wsocketMsg.ClientData.IsConnSign))
                {
                    chatService.CreateChatInfo(wsocketMsg);
                }
                else
                {
                    string clientName = wsocketMsg.CIp + ":" + wsocketMsg.CPort;
                    string traceInfo = string.Format("{0} 加入聊天室(共{1}人在线)", clientName, helper.PlayerCount);
                    wsocketMsg.ClientData.SMsg = traceInfo;

                }
                //立刻反馈
                helper.SendMessageToAll(wsocketMsg);
                return 1;
            };

            helper.ResponseTextEvent += (wsocketMsg) => {                 
                string jsonStr = Utils.ObjectToJsonStr(wsocketMsg);
                return jsonStr;
            };

            helper.WebSocketInit();
        }
    }
}
