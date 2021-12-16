using ChatApp.Factory;
using ChatApp.Interface.Message;
using System;

namespace ChatApp.Message
{
    public class Message
    {
        private IMessage messageRepository;
        private string self;
        private string to;
        private string content;

        public Message(string self, string activeChat, string content, IMessage messageRepository = null)
        {
            if (messageRepository == null) this.messageRepository = MessageFactory.CreateMessage();
            else this.messageRepository = messageRepository;

            this.self = self;
            this.to = activeChat;
            this.content = content;
        }

        internal void AddToDatabase()
        {
            MessageDTO dto = new MessageDTO { Message = content, SentTo = to, Username = self, Type = SentType.User };
            messageRepository.AddToDatabse(dto);
        }
    }
}
