using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    public class EFPager<T,OrderType>
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
            private set;
        }

        public int TotalPages
        {
            get;
            private set;
        }

        public Expression<Func<T, bool>> WhereLambds
        {
            get;
            set;
        }

        public bool IsAsc
        {
            get;
            set;
        }

        public Expression<Func<T, OrderType>> OrderByLambds
        {
            get;
            set;
        }

        public void SetTotalCount(int count)
        {
            this.TotalCount = count;
        }

        public void SetTotalPages(int pagesCount)
        {
            this.TotalPages = pagesCount;
        }


    }
}
