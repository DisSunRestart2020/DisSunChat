using DisSunChat.Repos.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Repos
{
    public class ChatDbContext:DbContext
    {
        public ChatDbContext() : base("name=MyStrConn")
        {
            //Database.SetInitializer<ChatDbContext>(null);//取消当数据库模型发生改变时删除当前数据库重建新数据库的设置
        }
        public DbSet<ChatHistories> ChatRecords { get; set; }
    }
}
