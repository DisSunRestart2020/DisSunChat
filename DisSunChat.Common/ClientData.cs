using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Common
{
    /// <summary>
    /// 客户发送的通信内容
    /// </summary>
    [Serializable]
    public class ClientData
    {
        /// <summary>
        /// 客户机器唯一标识
        /// </summary>
        public string IdentityMd5
        {
            get;
            set;
        }
        /// <summary>
        /// 客户发送的文本
        /// </summary>
        public string SMsg
        {
            get;
            set;
        }
        /// <summary>
        /// 客户头像代码
        /// </summary>
        public string ImgIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 是否是第一次连通的通知信号：1是，0否
        /// </summary>
        public string IsConnSign
        {
            get;
            set;      
        }
    }
}
