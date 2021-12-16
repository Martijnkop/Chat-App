using ChatApp.Database;
using ChatApp.Interface.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Factory
{
    public class MessageFactory
    {
        public static IMessage CreateMessage()
        {
            return new MessageRepository();
        }

        public static IMessageContainer CreateMessageContainer()
        {
            return new MessageRepository();
        }
    }
}
