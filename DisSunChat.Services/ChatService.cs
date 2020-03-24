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
        public int CreateChatInfo( string clientName,string chatContent)
        {
            ChatHistories history = new ChatHistories();
            history.ChatContent = chatContent;
            history.ClientName = clientName;
            history.CreateTime = DateTime.Now;

            SaveEntity(history);
            return 1;
        }
    }
}
