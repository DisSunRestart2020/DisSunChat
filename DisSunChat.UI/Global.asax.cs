
using DisSunChat.Common;
using DisSunChat.MvcFilter;
using DisSunChat.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            App_Start.FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            ChatService chatService = new ChatService();
            try
            {

                IWebSocketHelper helper = new FleckHelper();
                helper.WsOpenEventHandler += () =>
                {
                   Utils.SaveLog("WebSocket已经开启");
                    return 1;
                };

                helper.WsCloseEventHandler += () =>
                {
                    Utils.SaveLog("WebSocket已经关闭");
                    return 1;
                };

                helper.ListenEventHandler += (wsocketMsg) =>
                {
                    Utils.SaveLog("WebSocket监听到了消息");
                    if (!Convert.ToBoolean(wsocketMsg.ClientData.IsConnSign))
                    {
                        chatService.CreateChatInfo(wsocketMsg);
                        wsocketMsg.ClientData.SMsg = Utils.ReplaceIllegalWord(wsocketMsg.ClientData.SMsg);
                        //string cctv = Utils.ReplaceIllegalWord(wsocketMsg.ClientData.SMsg);
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

                helper.ResponseTextEventHandler += (wsocketMsg) =>
                {
                    string jsonStr = Utils.ObjectToJsonStr(wsocketMsg);
                    return jsonStr;
                };

                helper.WebSocketInit();
            }
            catch(Exception ex)
            {
                Utils.SaveLog("发现了错误：" + ex.Message);
            }
        }


        
    }
}
