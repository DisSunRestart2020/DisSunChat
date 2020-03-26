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

            //chatServier.CreateChatInfo("客户端" + DateTime.Now.Ticks, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));


            return View();
        }

        public ActionResult ChatPage()
        {
            return View();
        }

        public ActionResult DiyLayout()
        {
            return View();
        }
    }
}