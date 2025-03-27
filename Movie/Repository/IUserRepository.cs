using Movie.Models;
using Microsoft.AspNetCore.Mvc;

namespace Movie.Repository
{
    public interface IUserRepository
    {
        // Lấy tất cả người dùng với phân trang, tìm kiếm, và sắp xếp
        Task<ActionResult<IEnumerable<RequestUserDTO>>> GetAllUsersAsync(
            string? search = null,
            string sortBy = "id",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 5
        );

        // Lấy thông tin người dùng theo ID
        Task<RequestUserDTO> GetUserByIdAsync(int id);

        // Lấy thông tin người dùng theo Username
        Task<RequestUserDTO> GetUserByUserNameAsync(string username);

        // Lấy thông tin người dùng theo Email
        Task<RequestUserDTO> GetUserByEmailAsync(string email);

        // Tạo mới một người dùng
        Task<RequestUserDTO?> CreateUserAsync(string username, string email, string password);

        // Cập nhật thông tin người dùng
        Task<RequestUserDTO?> UpdateUserAsync(int id, string username, string email, string password);

        // Xóa người dùng
        Task<bool> PermanentlyDeleteUserAsync(int id);

        // Cập nhật trạng thái người dùng (1: active, 0: deactivated)
        Task<bool> UpdateUserStatusAsync(int id, int status);

        // Lịch sử xóa người dùng (Status = 0)
        Task<List<RequestUserDTO>> GetUsersDeleteHistoryAsync(
            string? search = null,
            string sortBy = "id",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 5
        );

        // Lưu thay đổi
        Task<bool> SaveChangesAsync();

        // Đăng nhập người dùng và trả về token
        Task<string?> LoginUserAsync(string email, string password);
    }
}
