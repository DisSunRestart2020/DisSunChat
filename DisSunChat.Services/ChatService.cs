using DisSunChat.Common;
using DisSunChat.Repos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Services
{
    public class ChatService:BaseService
    {
        public int CreateChatInfo(WebSocketMessage wsocketMsg)
        {
            ChatHistories history = new ChatHistories();
            history.ChatContent = wsocketMsg.ClientData.SMsg;
            history.ClientName = wsocketMsg.CIp+":"+ wsocketMsg.CPort;
            history.CreateTime = Convert.ToDateTime(wsocketMsg.ChatTime);

            SaveEntity(history);
            return 1;
        }
    }
}
