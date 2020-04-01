using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    public class UIPager
    {
        public int  PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int TotalPages
        {
            get;
            set;
        }

        public object ReponseObj
        {
            get;
            set;
        }
         


    }
}
