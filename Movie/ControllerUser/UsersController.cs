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

        // POST: api/User/SignUp
        [HttpPost("Sign-Up")]
        public async Task<ActionResult<RequestUserDTO>> CreateUser(RequestUserDTO requestUser)
        {
            try
            {
                var createdUser = await _userRepository.CreateUserAsync(requestUser.Username, requestUser.Email, requestUser.Password);

                if (createdUser == null)
                {
                    return BadRequest(new { Message = "Tên tài khoản hoặc email đã tồn tại." });
                }

                return CreatedAtAction(nameof(GetUsers), new { id = createdUser.UserId }, createdUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        private object GetUsers()
        {
            throw new NotImplementedException();
        }

        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            if (userDTO == null)
                return BadRequest("Dữ liệu người dùng không hợp lệ");

            var user = await _userRepository.GetUserByUserNameAsync(userDTO.UserName);

            if (user == null || user.Password != userDTO.Password)
                return Unauthorized("Tên người dùng hoặc mật khẩu không hợp lệ");

            return Ok("Đăng nhập thành công");
        }
    }

}