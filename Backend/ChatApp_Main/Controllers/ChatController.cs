using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ChatApp.Interface.User;
using ChatApp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ChatApp.User;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Linq;
using ChatApp.Interface.Message;
using ChatApp.Message;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration config;

        public ChatController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpPost]
        [Route("/registeruser")]
        public bool RegisterUser([FromBody] UserDTO data)
        {
            string REGISTERUSERSECRET = "";
            // if (secret == null || secret != REGISTERUSERSECRET) return false;

            return new UserContainer().CreateUser(data).AddToDatabase();
            return false;
        }

        [HttpGet]
        [Route("/friends")]
        public string GetFriends([FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token)) return "";
            if (!new TokenService(config).ValidateToken(token, out SecurityToken tokenOut))
            {
                return "";
            }

            List<UserDTO> u = new UserContainer().GetAllUsersAsDTO();
            return JsonConvert.SerializeObject(u);
        }

        [HttpGet]
        [Route("/getactivemessages/{user}")]
        public List<FrontEndMessage> GetActiveMessagesByUser(string user, [FromHeader] string token)
        {
            if (string.IsNullOrEmpty(token)) return null;
            if (!new TokenService(config).ValidateToken(token, out SecurityToken tokenOut))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var decoded = handler.ReadJwtToken(token);

            string self = decoded.Claims.First(claim => claim.Type == "name").Value;

            List<MessageDTO> messages = new MessageContainer().GetAllByUsernames(user, self);

            return MessageContainer.ReplaceToFrontEndMessages(messages);
        }
    }
}
