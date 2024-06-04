using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoruCevap.Models;
using SoruCevap.Service.Abstraction;
using SoruCevapApi.Models;
using SoruCevapApi.Models.ViewModel;

namespace SoruCevapApi.Controllers
{
    [Route("api/Sign")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly ISignService _signService;

        public SignController(ISignService signService)
        {
            _signService = signService;
        }

        [HttpPost("Up")]
        public async Task<IActionResult> SignUp(RegisterModel model)
        {
            var result =  await _signService.SaveUser(model);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message, errorCode = result.errorCode });
        }

        [HttpPost("In")]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
            var result = await _signService.GetUser(model);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errorCode = result.errorCode });
        }

        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var result = _signService.GetUsers();
            return Ok(result);
        }

        [HttpDelete("Users")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _signService.DeleteUser(id);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result); // veya diğer uygun bir hata kodu
            }
        }
    }
}
