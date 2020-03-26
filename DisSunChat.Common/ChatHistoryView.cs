using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    public class ChatHistoryView
    {
       
        public string ClientName
        {
            get;
            set;
        }

        
        public string CreateTime
        {
            get;
            set;
        }
        
        public string ChatContent
        {
            get;
            set;

        }
        
        public string IdentityMd5
        {
            get;
            set;
        }
        
        public string ImgIndex
        {
            get;
            set;
        }
    }
}
