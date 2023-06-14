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
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            User user = await userRespository.GetByUsername(loginRequest.Username!);
            if (user == null) return Unauthorized();

            bool isCorrectPassword = passwordHasher.VerifyPassword(loginRequest.Password!, user.Password!);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }
        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
