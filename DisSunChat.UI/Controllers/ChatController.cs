using DisSunChat.Common;
using DisSunChat.Repos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisSunChat.Controllers
{
    public class ChatController : BaseController
    {
        /// <summary>
        /// 聊天室首页
        /// </summary>
        /// <returns></returns>
        // GET: Chat
        public ActionResult Index()
        {    
            return View();
        }

        /// <summary>
        /// 获取数据集列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult GetDataList(int pageIndex,int pageSize)
        {
            //需要的数据：总共多少条数据、多少页、当前页、排序方式
            List<ChatHistoryView> dataList = chatServier.GetDataList(pageIndex, pageSize);
            var items= dataList.OrderBy(x => x.CreateTime).ToList();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}