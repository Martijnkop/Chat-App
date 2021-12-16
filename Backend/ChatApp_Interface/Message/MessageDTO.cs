using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Interface.Message
{
    public class MessageDTO
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string SentTo { get; set; }
        public SentType Type { get; set; } = SentType.User;
        public string Message { get; set; }
    }

    public enum SentType
    {
        User,
        Group,
        Channel
    }
}
