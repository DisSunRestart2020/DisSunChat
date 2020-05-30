
using DisSunChat.Common;
using DisSunChat.MvcFilter;
using DisSunChat.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
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
                helper.WebSocketInit();
                  
                helper.WsOpenEventHandler +=(sender, args)=>{
                    Utils.SaveLog("WebSocket已经开启2");
                };

                helper.WsCloseEventHandler += (sender, args) => {
                    Utils.SaveLog("WebSocket已经关闭2");
                };

                helper.WsListenEventHandler += (sender, args) => {
                    Utils.SaveLog("WebSocket监听到了消息2");
                    if (!Convert.ToBoolean(args.WebSocketMessage.ClientData.IsConnSign))
                    {
                        chatService.CreateChatInfo(args.WebSocketMessage);
                        args.WebSocketMessage.ClientData.SMsg = Utils.ReplaceIllegalWord(args.WebSocketMessage.ClientData.SMsg);
                    }
                    else
                    {
                        string clientName = args.WebSocketMessage.CIp + ":" + args.WebSocketMessage.CPort;
                        string traceInfo = string.Format("{0} 加入聊天室(共{1}人在线)", clientName, helper.PlayerCount);
                        args.WebSocketMessage.ClientData.SMsg = traceInfo;
                    }
                    //立刻反馈                   
                    helper.SendMessageToAll(args.WebSocketMessage);
                };

                helper.WsResponseTextEventHandler += (sender, args) => {
                    string jsonStr = Utils.ObjectToJsonStr(args.WebSocketMessage);
                    args.ResultDataMsg = jsonStr;
                };

               
            }
            catch(Exception ex)
            {
                Utils.SaveLog("发现了错误：" + ex.Message);
            }
        }


        
    }
}
