using Identity.Dto;
using Identity.Service;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController : ControllerBase
    {
        private readonly IAuthService _authService;
        public ConnectController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null)
            {
                return BadRequest("Invalid username or password");
            }
            return Ok(response);
        }
    }
}
