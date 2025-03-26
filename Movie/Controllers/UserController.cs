
using System.Drawing.Printing;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Movie.Repository;
using Movie.RequestDTO;
using Movie.Models;

namespace Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // POST: api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(string email, string password)
        {
            var token = await _userRepository.LoginUserAsync(email, password);

            if (token == null)
            {
                return Unauthorized(new { Messgae = "Tên tài khoản hoặc mật khẩu không tồn tại" });
            }

            return Ok(new { Message = "Đăng nhập thành công" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(string username, string newPassword)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return NotFound(new { Message = "Người dùng hong tồn tại." });
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateUserAsync(user.UserId, user.Username, user.Email, hashedPassword);

            return Ok(new { Message = "Mật khẩu đã được cập nhật thành công." });
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestUserDTO>>> GetUsers(
            string? search = null,
            string sortBy = "id",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 5)
        {
            var users = await _userRepository.GetAllUsersAsync(search, sortBy, sortDirection, page, pageSize);
            return users;
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestUserDTO>> GetUser(int id)
        {
            // Lấy thông tin người dùng theo id và chỉ lấy những người có Status = 1
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null || user.Status != 1)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng hoặc người dùng đã bị xóa." });
            }

            return Ok(user);
        }
        // POST: api/User/SignUp
        [HttpPost]
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

            // PUT: api/Users/{id}
            [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(
            int id,
            string username,
            string email,
            string password)
        {
            try
            {
                var updatedUser = await _userRepository.UpdateUserAsync(id, username, email, password);

                if (updatedUser == null)
                {
                    return NotFound(new { Message = "Không tìm thấy người dùng." });
                }

                return Ok(updatedUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            // Lấy thông tin người dùng từ repository
            var user = await _userRepository.GetUserByIdAsync(id);

            // Kiểm tra xem người dùng có tồn tại không
            if (user == null)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng." });
            }

            // Kiểm tra nếu người dùng đã bị xóa rồi (Status = 0)
            if (user.Status == 0)
            {
                return BadRequest(new { Message = "Người dùng đã bị xóa trước đó." });
            }

            // Nếu người dùng còn sống (Status = 1), thay đổi trạng thái thành 0 (đã xóa)
            user.Status = 0;

            // Cập nhật lại trạng thái người dùng trong cơ sở dữ liệu
            bool updateSuccess = await _userRepository.UpdateUserStatusAsync(user.UserId, user.Status);
            if (updateSuccess)
            {
                return Ok(new { Message = "Người dùng đã được xóa thành công" });
            }
            else
            {
                return BadRequest(new { Message = "xóa người dùng không thành công." });
            }
        }

        // GET: api/Users/lichsu-xoa
        [HttpGet("lichsu-xoa")]
        public async Task<IActionResult> GetUsersDeleteHistory(
            string? search = null,  // Tìm theo tên người dùng (Username)
            string sortBy = "id",   // Trường sắp xếp (mặc định theo id)
            string sortDirection = "asc", // Hướng phân loại (asc/desc)
            int page = 1,           // Số trang (mặc định là trang 1)
            int pageSize = 5        // Số mục trên mỗi trang (mặc định là 5)
        )
        {
            // Gọi phương thức trong repository để lấy danh sách người dùng đã xóa với phân trang, tìm kiếm và sắp xếp
            var users = await _userRepository.GetUsersDeleteHistoryAsync(search, sortBy, sortDirection, page, pageSize);

            if (users == null || users.Count == 0)
            {
                return NotFound(new { Message = "Không có người dùng nào đã bị xóa." });
            }

            return Ok(new
            {
                Users = users
            });
        }

        // DELETE: api/Users/Permanetly-Delete
        [HttpDelete("permanently-delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.PermanentlyDeleteUserAsync(id);

            if (!result)
            {
                return NotFound(new { Message = "Không tìm thấy người dùng." });
            }

            return Ok(new { Message = "Xóa người dùng vĩnh viễn thành công." });
        }


    }
}