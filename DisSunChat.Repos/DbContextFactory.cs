using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Repos
{
    public class DBContextFactory
    {
        /// <summary>
        /// 保证EF上下文实例是线程内唯一。
        /// </summary>
        /// <returns></returns>
        public DbContext CreateDbContext()
        {
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new ChatDbContext();

                CallContext.SetData("dbContext", dbContext);
            }
            return dbContext;
        }
    }
}
