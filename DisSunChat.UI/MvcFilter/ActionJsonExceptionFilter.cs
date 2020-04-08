using DisSunChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DisSunChat.MvcFilter
{
    public class ActionJsonExceptionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                Exception ex = filterContext.Exception.GetBaseException();
                StringBuilder str = new StringBuilder();
                str.Append("\r\n捕获控制器异常");
                str.Append("\r\n错误信息：" + ex.Message);
                str.Append("\r\n错误源：" + ex.Source);
                str.Append("\r\n异常方法：" + ex.TargetSite);
                str.Append("\r\n堆栈信息：" + ex.StackTrace);
                str.Append("\r\n-----------------------------------------------------------------");
                Utils.SaveLog(str.ToString());

                CommonResult<object> cr = new CommonResult<object>(CommonCode.Error, ex.Message);

                JsonResult jr = new JsonResult();
                jr.Data = cr;
                jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


                filterContext.Result = jr;
                //告诉系统，这个异常已经处理了，不用再处理
                filterContext.ExceptionHandled = true;
            }
        }

    }
}