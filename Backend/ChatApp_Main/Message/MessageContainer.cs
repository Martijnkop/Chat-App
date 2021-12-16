using ChatApp.Factory;
using ChatApp.Interface.Message;
using System;
using System.Collections.Generic;

namespace ChatApp.Message
{
    public class MessageContainer
    {
        private IMessageContainer messageRepository;
        public MessageContainer(IMessageContainer messageContainer = null)
        {
            if (messageContainer == null) messageRepository = MessageFactory.CreateMessageContainer();
            else messageRepository = messageContainer;
        }

        internal List<MessageDTO> GetAllByUsernames(string user, string self)
        {
            return messageRepository.GetAllByUsernames(user, self);
        }

        internal static List<FrontEndMessage> ReplaceToFrontEndMessages(List<MessageDTO> messages)
        {
            List<FrontEndMessage> frontEndMessages = new List<FrontEndMessage>();
            foreach (MessageDTO message in messages)
            {
                frontEndMessages.Add(new FrontEndMessage { Content = message.Message, SentTo = message.SentTo, Username = message.Username });
            }

            return frontEndMessages;
        }

        internal Message CreateMessage(string self, string activeChat, string content)
        {
            Message message = new Message(self, activeChat, content);
            return message;
        }
    }
}
