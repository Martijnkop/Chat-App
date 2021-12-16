using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public interface IMessageClient
    {
        public Task GetMessage();
    }
}
