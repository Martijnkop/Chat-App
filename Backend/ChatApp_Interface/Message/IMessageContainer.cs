using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Interface.Message
{
    public interface IMessageContainer
    {
        List<MessageDTO> GetAllByUsernames(string user, string self);
    }
}
