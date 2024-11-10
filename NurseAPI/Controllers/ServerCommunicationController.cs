using Microsoft.AspNetCore.Mvc;
using NurseAPI.Services;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NurseAPI.Models;

namespace NurseAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServerCommunicationController : Controller
    {
        private static List<string> ct = new List<string>();
        private int count = 0;

        [Route("checktoken")]
        [HttpPost]
        [Authorize]
        public string CheckAccess()
        {
            return "true";
        }

        [Route("gentoken")]
        [HttpPost]
        public ActionResult GenToken(Auth auth)
        {
            if (auth.password == Configuration.password)
            {
                var claims = new List<Claim> { new Claim("password", auth.password) };
                var jwt = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromDays(365)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                
                return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            else
            {
                return Unauthorized();
            }
        }

        [Route("SendTelegram")]
        [HttpPost]
        public StatusCodeResult Post(RequestMessage data)
        {
            try
            {
                if (!Configuration.developStatus || ct.Where(p => p.Contains(HttpContext.Connection.RemoteIpAddress.ToString())).Count() < Configuration.numberMessages + 1)
                {
                    ct.Add(HttpContext.Connection.RemoteIpAddress.ToString());
                    StartServer.IsNewMessages = true;
                    StartServer.textMessage = JsonSerializer.Serialize(data);
                    StartServer.SendMessage(StartServer.socket);
                    return Ok();
                }
                TimeSpan span = DateTime.Now.Subtract(new System.DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 13, 20, 0));
                if (span.TotalSeconds > 0 && span.TotalSeconds < 270)
                {
                    ct.Clear();
                    return StatusCode(4230);
                }
                return StatusCode(429);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
