using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartEnergyHub.API.Filters;
using SmartEnergyHub.API.Models;
using SmartEnergyHub.BLL.Auth.Interfaces;
using SmartEnergyHub.DAL.EF;
using SmartEnergyHub.DAL.Entities.APIUser;

namespace SmartEnergyHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            ITokenProvider tokenProvider)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException( nameof(tokenProvider));
        }

        //[Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return ExceptionFilter.ErrorResult(nameof(request.Username));
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ExceptionFilter.ErrorResult(nameof(request.Username));
            }

            User? user_ = _dbContext.Users.Where(x=>x.Username == request.Username).FirstOrDefault();

            if (user_ != null)
            {
                return BadRequest($"User with username '{request.Username}' is exist");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            User user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("token")]
        public IActionResult GetToken(UserRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
            {
                return ExceptionFilter.ErrorResult(nameof(request.Username));
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ExceptionFilter.ErrorResult(nameof(request.Username));
            }

            User? user = _dbContext.Users.Where(x => x.Username == request.Username).FirstOrDefault();

            if (user == null)
            {
                return NotFound($"User with username '{request.Username}' is not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong password");
            }

            string token = _tokenProvider.CreateToken(_configuration, user);

            return Ok(token);
        }
    }
}
