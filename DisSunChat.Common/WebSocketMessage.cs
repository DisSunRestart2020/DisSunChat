using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    public class WebSocketMessage
    {
        public string cIp
        {
            get;set;
        }
        public string cPort
        {
            get;
            set;
        }

        public string cGuid
        {
            get;
            set;
        }

        public string ChatTime
        {
            get;
            set;
        }

        public ClientData clientData
        {
            get;
            set;
        }
    }
}
