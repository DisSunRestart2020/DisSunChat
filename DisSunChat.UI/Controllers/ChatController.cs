using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DisSunChat.Controllers
{
    public class ChatController : Controller
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