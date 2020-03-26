using DisSunChat.Common;
using DisSunChat.Repos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            history.IdentityMd5 = wsocketMsg.ClientData.IdentityMd5;
            history.ImgIndex = wsocketMsg.ClientData.ImgIndex;            

            SaveEntity(history);
            return 1;
        }


        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<ChatHistoryView> GetDataList(int pageIndex,int pageSize)
        {
            IQueryable<ChatHistories> items = LoadEntities< ChatHistories>(x=> 1==1);
            items=items.OrderByDescending(x => x.CreateTime).Skip(pageIndex * pageSize).Take(pageSize);

            List<ChatHistoryView> list = new List<ChatHistoryView>();
            foreach (var it in items)
            {
                ChatHistoryView view = new ChatHistoryView();
                view.ChatContent = it.ChatContent;
                view.CreateTime = it.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                view.ClientName = it.ClientName;
                view.IdentityMd5 = it.IdentityMd5;
                view.ImgIndex = it.ImgIndex;
                list.Add(view);
            }

            return list;


           
        }
    }
}
