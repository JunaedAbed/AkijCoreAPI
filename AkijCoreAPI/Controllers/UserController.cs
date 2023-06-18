using AkijCoreAPI.Models.Requests;
using AkijCoreAPI.Models;
using AkijCoreAPI.Services.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AkijCoreAPI.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRespository userRespository;

        public UserController(IUserRespository userRespository)
        {
            this.userRespository = userRespository;
        }

        [HttpGet("userAuthorization")]
        public IActionResult UserAuthorization()
        {
            return Ok("Authorized");
        }
    }
}
