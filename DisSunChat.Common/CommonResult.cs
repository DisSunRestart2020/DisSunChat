using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    /// <summary>
    /// 公共的结果返回实体，凡是对外的Api接口，都以这种形式返回，当code=-1时证明接口异常，方便向调用方提供明确又不失安全的诊断信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResult<T>
    {
        public CommonResult(int code,string msg)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = default(T);
        }

        public CommonResult(int code, string msg,T t)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = t;
        }


        public int Code
        {
            get;
            set;
        }
        public string Msg
        {
            get;
            set;
        }

        public T Data
        {
            get;
            set;
        }


    }
}
