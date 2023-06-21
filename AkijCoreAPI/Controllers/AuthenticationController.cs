using AkijCoreAPI.Models;
using AkijCoreAPI.Models.Requests;
using AkijCoreAPI.Models.Responses;
using AkijCoreAPI.Services.Authenticators;
using AkijCoreAPI.Services.PasswordHashers;
using AkijCoreAPI.Services.RefreshTokenRepositories;
using AkijCoreAPI.Services.TokenGenerators;
using AkijCoreAPI.Services.TokenValidators;
using AkijCoreAPI.Services.UserRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AkijCoreAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        #region private
        private readonly IUserRespository userRespository;
        private readonly IPasswordHasher passwordHasher;
        private readonly Authenticator authenticator;
        private readonly RefreshTokenValidator refreshTokenValidator;
        private readonly IRefreshTokenRepository refreshTokenRepository;

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage));
            return BadRequest(new ErrorResponse(errorMessages));
        }
        #endregion

        public AuthenticationController(IUserRespository userRespository, IPasswordHasher passwordHasher, AccessTokenGenerator accessTokenGenerator, RefreshTokenGenerator refreshTokenGenerator, RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepository refreshTokenRepository, Authenticator authenticator)
        {
            this.userRespository = userRespository;
            this.passwordHasher = passwordHasher;
            this.refreshTokenValidator = refreshTokenValidator;
            this.refreshTokenRepository = refreshTokenRepository;
            this.authenticator = authenticator;
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

            User user = await userRespository.GetByUsername(loginRequest.Username);
            if (user == null) return Unauthorized();

            bool isCorrectPassword = passwordHasher.VerifyPassword(loginRequest.Password, user.Password);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponse response = await authenticator.Authenticate(user);

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValidRefreshToken = refreshTokenValidator.Validate(refreshRequest.RefreshToken);
            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }

            RefreshToken refreshTokenDTO = await refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);
            if (refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token"));
            }

            await refreshTokenRepository.Delete(refreshTokenDTO.Id);

            User user = await userRespository.GetById(refreshTokenDTO.Uid);
            if (user == null)
            {
                return NotFound(new ErrorResponse("User not found"));
            }

            AuthenticatedUserResponse response = await authenticator.Authenticate(user);

            return Ok(response);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string userId = HttpContext.User.FindFirstValue("id");

            if(!int.TryParse(userId, out int uid))
            {
                return Unauthorized();
            }

            await refreshTokenRepository.DeleteAll(int.Parse(userId));

            return NoContent();
        }
    }
}
