
using DisSunChat.Common;
using DisSunChat.Services;
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
                Utils.SaveLog("WebSocket已经开启");
                return 1;
            };

            helper.WsCloseEvent += () => {
                Utils.SaveLog("WebSocket已经关闭");
                return 1;
            };

            helper.ListenEvent += (d, c) =>
            {
                chatService.CreateChatInfo(c, d);
                return 1;
            };

            helper.ResponseEvent += (d, c) => {
                string timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string jsonStr = "{\"ClientName\":\"" + c + "\",\"ChatTime\":\"" + timeStr + "\",\"ChatMsg\":\"" + d + "\"}";
                return jsonStr;
            };

            helper.WebSocketInit();
        }
    }
}
