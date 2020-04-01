using DisSunChat.Common;
using DisSunChat.Repos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public UIPager GetDataList(int pageIndex,int pageSize)
        {
            UIPager uip = new UIPager();

            EFPager< ChatHistories,DateTime> pager = new EFPager<ChatHistories, DateTime>();
            pager.PageIndex = pageIndex;
            pager.PageSize = pageSize;
            //pager.WhereLambds = x => DbFunctions.DiffDays(x.CreateTime, DateTime.Now) == 0;
            pager.WhereLambds = x => 1==1;
            pager.IsAsc = false;
            pager.OrderByLambds = o => o.CreateTime;

            IQueryable<ChatHistories> items = LoadEntities<ChatHistories, DateTime>(pager);

            uip.PageSize = pager.PageSize;
            uip.PageIndex = pager.PageIndex;
            uip.TotalCount = pager.TotalCount;
            uip.TotalPages = pager.TotalPages;       

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

            uip.ReponseObj = list.OrderBy(x => x.CreateTime).ToList();
            return uip;           
        }
    }
}
