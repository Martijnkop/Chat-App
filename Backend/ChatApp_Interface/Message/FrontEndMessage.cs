using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Interface.Message
{
    public class FrontEndMessage
    {
        public string Username { get; set; }
        public string SentTo { get; set; }
        public string Content { get; set; }
    }
}
