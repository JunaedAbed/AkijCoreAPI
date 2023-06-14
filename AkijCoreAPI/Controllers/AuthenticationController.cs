using AkijCoreAPI.Models;
using AkijCoreAPI.Models.Requests;
using AkijCoreAPI.Models.Responses;
using AkijCoreAPI.Services.PasswordHashers;
using AkijCoreAPI.Services.UserRepositories;
using Microsoft.AspNetCore.Mvc;

namespace AkijCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRespository userRespository;
        private readonly IPasswordHasher passwordHasher;

        public AuthenticationController(IUserRespository userRespository, IPasswordHasher passwordHasher)
        {
            this.userRespository = userRespository;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));  

            if (!ModelState.IsValid) return BadRequest(new ErrorResponse(errorMessages));

            if (registerRequest.Password != registerRequest.ConfirmPassword) return BadRequest(new ErrorResponse("Password does not match confirm password"));

            User existingUserByEmail = await userRespository.GetByEmail(registerRequest.Email!);
            if (existingUserByEmail != null) return Conflict(new ErrorResponse("Email already registered"));

            User existingUserByUsername = await userRespository.GetByUsername(registerRequest.Username!);
            if (existingUserByUsername != null) return Conflict(new ErrorResponse("Username already registered"));

            User registerUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                Password = passwordHasher.HashPassword(registerRequest.Password!),
            };

            await userRespository.CreateUser(registerUser);

            return Ok(registerUser);
        }
    }
}
