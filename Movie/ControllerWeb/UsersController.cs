using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Movie.Models;
using Movie.Repository;
using Movie.ResponseDTO;


namespace Movie.ControllerWeb
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // POST: api/User/SignUp-User
        [HttpPost("SignUp-User")]
        public async Task<ActionResult<RequestUserDTO>> CreateUser(
            string username,
            string email,
            string password)
        {
            try
            {
                // Tạo tài khoản mới
                var createdUser = await _userRepository.CreateUserAsync(username, email, password);

                if (createdUser == null)
                {
                    // Trả về lỗi nếu tên tài khoản hoặc email đã tồn tại
                    return BadRequest(new { Message = "Tên tài khoản hoặc email đã tồn tại." });
                }

                // Trả về thông báo thành công cùng thông tin người dùng
                return Ok(new { Message = "Tài khoản đã được tạo thành công.", User = createdUser });
            }
            catch (ArgumentException ex)
            {
                // Trả về lỗi nếu có bất kỳ ngoại lệ nào
                return BadRequest(new { ex.Message });
            }
        }

        // POST: api/User/login-User
        [HttpPost("login-User")]
        public async Task<IActionResult> LoginUser(string email, string password)
        {
            var token = await _userRepository.LoginUserAsync(email, password);

            if (token == null)
            {
                return Unauthorized(new { Messgae = "Tên tài khoản hoặc mật khẩu không tồn tại" });
            }

            return Ok(new { Message = "Đăng nhập thành công" });
        }
    }

}