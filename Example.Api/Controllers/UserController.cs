using System.Threading.Tasks;
using Example.Api.Security;
using Example.Models.Models;
using Example.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Example.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Policy = PolicyConstants.AnyRole)]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IConfiguration configuration, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        ///     get user by id
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{userId}")]
        public Task<UserDto> GetById(int userId)
        {
            return _userService.GetById(userId);
        }

        /// <summary>
        ///     create user
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = PolicyConstants.OnlyAdmin)]
        [HttpPost]
        public Task<UserDto> Create(UserCreateDto input)
        {
            return _userService.Create(input);
        }

        /// <summary>
        ///     edit user by id
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = PolicyConstants.OnlyAdmin)]
        [HttpPut("{userId}")]
        public Task<UserDto> Edit(int userId, UserEditDto input)
        {
            return _userService.Edit(userId, input);
        }

        /// <summary>
        ///     delete user by id
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = PolicyConstants.OnlyAdmin)]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            await _userService.Delete(userId);

            return NoContent();
        }
    }
}
