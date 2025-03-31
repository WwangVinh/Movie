using Microsoft.AspNetCore.Mvc;
using Movie.Models;
using Movie.Repository;
using Movie.ResponseDTO;


namespace Movie.ControllerUser
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
        [HttpPost("SignUp-User")]
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
        [HttpPost("login-User")]

        public async Task<IActionResult> LoginUser(RequestUserDTO requestUser)

        {

            var token = await _userRepository.LoginUserAsync(requestUser.Email, requestUser.Password);

            if (token == null)

            {

                return Unauthorized(new { Messgae = "Tên tài khoản hoặc mật khẩu không tồn tại" });

            }

            return Ok(new { Message = "Đăng nhập thành công" });

        }


    }

}