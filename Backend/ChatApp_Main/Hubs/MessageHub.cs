using ChatApp.Message;
using ChatApp.User;
using ChatApp.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class MessageHub : Hub<IMessageClient>
    {
        private readonly IConfiguration config;

        public MessageHub(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendMessage(string token, string activeChat, string message)
        {
            if (string.IsNullOrEmpty(token)) return;
            if (!new TokenService(config).ValidateToken(token, out SecurityToken tokenOut))
            {
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var decoded = handler.ReadJwtToken(token);

            string self = decoded.Claims.First(claim => claim.Type == "name").Value;

            new MessageContainer().CreateMessage(self, activeChat, message).AddToDatabase();
            string connectionID = new UserContainer().FindUserByUsername(activeChat).ConnectionID;

            try
            {
                await Clients.Clients(connectionID, Context.ConnectionId).GetMessage();
                await Clients.Caller.GetMessage();
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SaveSessionID(string username)
        {
            Console.WriteLine(Context.ConnectionId);
            Console.WriteLine(username);

            new UserContainer().FindUserByUsername(username).SetConnectionID(Context.ConnectionId);
        }
    }
}
