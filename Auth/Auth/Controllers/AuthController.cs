using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auth.Interface.Token;
using Auth.Interface.User;
using Auth.Logic.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Auth.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration config;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;
        private readonly UserContainer userContainer;

        public AuthController(IConfiguration config, ITokenService tokenService, IUserRepository userRepository)
        {
            this.tokenService = tokenService;
            this.userRepository = userRepository;
            this.config = config;
            this.userContainer = new UserContainer(userRepository);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [Route("register")]
        public IActionResult Register(AccountRegister register)
        {
            if (register.Email == null || register.Username == null || register.FirstName == null || register.LastName == null || register.Password == null)
            {
                ValidationProblem();
            }

            User user = new UserContainer().CreateUser(register);

            if (user.ExistsInDatabase())
            {
                return Forbid();
            }

            char[] refreshToken = GenerateRefreshToken();

            user.SetRefreshToken(refreshToken);
            user.AddToDatabase();

            Console.WriteLine(HttpContext);

            SendRegisterData(register);

            return Login(new AccountLogin { Email = register.Email, Password = register.Password });
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [Route("/login")]
        public IActionResult Login(AccountLogin login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest();
            }

            var validUser = userContainer.GetAsDTO(login);

            if (validUser.Id != Guid.Empty)
            {
                User user = new UserContainer().CreateUser(login);

                char[] refreshToken = GenerateRefreshToken();

                user.SetRefreshToken(refreshToken);
                user.Update("refreshToken");

                return ReturnAccessTokenAsActionResult(validUser, refreshToken);
            }
            return NotFound();
        }

        private IActionResult ReturnAccessTokenAsActionResult(UserDTO validUser, char[] refreshToken)
        {
            if (validUser == null) return StatusCode(404);
            string generatedToken = tokenService.BuildToken(config["Jwt:Key"].ToString(), config["Jwt:Issuer"].ToString(), validUser);

            if (generatedToken != null)
            {
                var returnObject = new { generatedToken = generatedToken, refreshToken = refreshToken };

                return Ok(returnObject);
            }
            else
            {
                return StatusCode(409);
            }
        }

        [HttpGet]
        [Route("/generatewithrefreshtoken")]
        public IActionResult RefreshTokenLogin([FromHeader] string token)
        {
            // Logout: NotFound()
            // Login: BadRequest()
            // App (valid): Ok()

            if (string.IsNullOrEmpty(token)) return BadRequest();
            char[] refreshToken = token.ToCharArray();

            Console.WriteLine(refreshToken);
            if (refreshToken == null || refreshToken.Length != 255) return BadRequest();

            UserDTO u = userContainer.GetUserByRefreshToken(refreshToken);

            return ReturnAccessTokenAsActionResult(u, refreshToken);
        }

        [HttpGet]   
        [Route("/getaudiencetoken/{audience}")]
        public string GetAudienceToken(string audience, [FromHeader] string token)
        {
            if (ValidateToken(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var decoded = handler.ReadJwtToken(token);
                Console.WriteLine(decoded);

                string userName = decoded.Claims.First(claim => claim.Type == "name").Value;
                Guid id = Guid.Parse(decoded.Claims.First(claim => claim.Type == "id").Value);

                UserDTO dto = new UserDTO { UserName = userName, Id = id};
                string generatedToken = tokenService.BuildToken(config["Jwt:Key"].ToString(), config["Jwt:Issuer"].ToString(), dto, audience);

                return generatedToken;
            }

            return "";
        }

        [HttpGet]
        [Route("/tokenlogin")]
        public bool TokenLogin([FromHeader] string accessToken)
        {
            if (String.IsNullOrWhiteSpace(accessToken)) return false;

            return ValidateToken(accessToken);
        }

        [HttpGet]
        [Route("/logout")]
        public bool Logout([FromHeader] string refreshToken)
        {
            string test = refreshToken.Replace("\\\\", "\\");

            if (test == null) return false;
            return userContainer.LogoutUser(test.ToCharArray());
        }

        private bool ValidateToken(string token)
        {
            return tokenService.ValidateToken(config["Jwt:Key"].ToString(), config["Jwt:Issuer"].ToString(), config["Jwt:Audience"].ToString(), token);
        }

        private char[] GenerateRefreshToken()
        {
            int start = 33;
            int end = 126;
            char[] chars = new char[255];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)new Random().Next(start, end);
            }

            return chars;
        }

        private async void SendRegisterData(AccountRegister register)
        {
            ShareProfile p = new ShareProfile { Username = register.Username, ConnectionID = "0" };

            var json = JsonConvert.SerializeObject(p);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync("https://localhost:5001/registeruser", data);

            string responseString = await response.Content.ReadAsStringAsync();
        }

        public enum Redirection
        {
            Main = 1,
            Login = 2,
            Register = 3,
            Logout = 4
        }

        /// Remove refresh token on logout
        /// GLOBAL delete refresh tokens
        /// Add refresh token to login
        /// Change JWT to use private public key pair
    }
}
