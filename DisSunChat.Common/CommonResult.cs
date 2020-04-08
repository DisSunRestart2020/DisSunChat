using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
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
