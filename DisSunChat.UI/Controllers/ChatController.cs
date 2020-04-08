using DisSunChat.Common;
using DisSunChat.MvcFilter;
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
            //throw new Exception("控制器出现异常了");
            return View();
        }

        /// <summary>
        /// 获取数据集列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ActionJsonExceptionFilter]
        public ActionResult GetDataList(int pageIndex,int pageSize)
        {
            UIPager pager = chatServier.GetDataList(pageIndex, pageSize);
            CommonResult<UIPager> cr = new CommonResult<UIPager>(CommonCode.Success, "请求成功", pager);
            //throw new Exception("Action出现了问题");
            return Json(cr, JsonRequestBehavior.AllowGet);
        }
    }
}