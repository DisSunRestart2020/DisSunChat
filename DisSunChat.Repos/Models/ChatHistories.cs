using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisSunChat.Repos.Models
{
    public class ChatHistories
    {
        [Key]
        [Display(Name = "主键")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int HID
        {
            get;
            set;
        }

        [Display(Name = "客户端来源")]
        [StringLength(200, ErrorMessage = "长度不可超过200")]
        public string ClientName
        {
            get;
            set;
        }

        [Display(Name = "接收时间")]
        public DateTime CreateTime
        {
            get;
            set;
        }

        [Display(Name = "聊天内容")]
        [StringLength(1000, ErrorMessage = "长度不可超过1000")]
        public string ChatContent
        {
            get;
            set;

        }
    }
}
