using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.Service;
using BCrypt.Net;
using Microsoft.CodeAnalysis.Scripting;
using Movie.Repository;

namespace Movie.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly movieDB _context;
        private int id;
        private readonly JwtService _jwtService;

        public UserRepository(movieDB context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<ActionResult<IEnumerable<RequestUserDTO>>> GetAllUsersAsync(
            string? search = null,  // Tìm theo tên người dùng (Username)
            string sortBy = "id", // Trường sắp xếp (mặc định theo id)
            string sortDirection = "asc", // Hướng phân loại (asc/desc)
            int page = 1, // Số trang (mặc định là trang 1)
            int pageSize = 5 // Số mục trên mỗi trang (mặc định là 5)
        )
        {
            // Kiểm tra nếu page hoặc pageSize nhỏ hơn 1, đặt giá trị mặc định
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 5;

            // Khởi tạo query cơ bản cho bảng Users, bao gồm Payments 
            var query = _context.Users
                               .Where(u => u.Status == 1)  // Chỉ lấy người dùng có Status = 1
                               .Include(u => u.Payments)
                               .AsQueryable();

            // Lọc theo tên người dùng (Username) nếu có search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => EF.Functions.Like(u.UserName, $"%{search}%"));
            }

            // Sắp xếp theo Username hoặc Email
            switch (sortBy?.ToLower())
            {
                case "name":
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserName)
                        : query.OrderBy(u => u.UserName);
                    break;

                case "email":
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Email)
                        : query.OrderBy(u => u.Email);
                    break;

                case "id":
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserId)
                        : query.OrderBy(u => u.UserId);
                    break;

                default:
                    // Mặc định sắp xếp theo id nếu sortBy không hợp lệ
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserId)
                        : query.OrderBy(u => u.UserId);
                    break;
            }

            // Lấy tổng số bản ghi để tính phân trang
            var totalRecords = await query.CountAsync();

            // Phân trang: Skip và Take
            var users = await query
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToListAsync();

            var result = users.Select(u => new RequestUserDTO
            {
                UserId = u.UserId,
                Username = u.UserName,
                Email = u.Email,
                Password = u.Password, // Nếu cần trả mật khẩu, nếu không thì có thể bỏ
                CreatedDate = u.CreatedDate,
                Status = u.Status
            }).ToList();

            // Kiểm tra nếu không có người dùng nào
            if (!users.Any())
            {
                return new NotFoundObjectResult(new { Message = "Không có người dùng nào được tìm thấy." });
            }

            // Trả về dữ liệu với thông tin phân trang
            return new OkObjectResult(new
            {
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                Users = result  // Trả lại danh sách người dùng đã được lọc
            });
        }

        public async Task<RequestUserDTO> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new RequestUserDTO
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                Password = user.Password,
                CreatedDate = user.CreatedDate,
                Status = user.Status // Thêm Status vào DTO
            };
        }

        public async Task<RequestUserDTO> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                return null;
            }
            return new RequestUserDTO
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                Password = user.Password,
                CreatedDate = user.CreatedDate
            };
        }

        public async Task<RequestUserDTO> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            return new RequestUserDTO
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email
            };
        }

        public async Task<RequestUserDTO?> CreateUserAsync(string username, string email, string password)
        {
            // Kiểm tra nếu email đã tồn tại
            if (_context.Users.Any(u => u.Email == email))
            {
                return null; // Email đã tồn tại
            }

            // Kiểm tra nếu username đã tồn tại
            if (_context.Users.Any(u => u.UserName == username))
            {
                return null;
            }


            // Kiểm tra độ dài mật khẩu (phải từ 6 đến 100 ký tự)
            if (password.Length < 6 || password.Length > 100)
            {
                throw new ArgumentException("Mật khẩu phải có từ 6 đến 100 ký tự.");
            }

            // Băm mật khẩu trc khi lưu vào Db
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                UserName = username,
                Email = email,
                Password = hashedPassword,
                CreatedDate = DateTime.Now,
                Status = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RequestUserDTO
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                CreatedDate = user.CreatedDate
            };
        }



        public async Task<RequestUserDTO?> UpdateUserAsync(int id, string username, string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return null; // Trả về null nếu không tìm thấy User
            }

            // Kiểm tra nếu email đã tồn tại ở User khác
            if (_context.Users.Any(u => u.Email == email && u.UserId != id))
            {
                throw new ArgumentException("Email này đã được sử dụng bởi tài khoản khác.");
            }

            // Kiểm tra nếu username đã tồn tại ở User khác
            if (_context.Users.Any(u => u.UserName == username && u.UserId != id))
            {
                throw new ArgumentException("Tên người dùng đã tồn tại.");
            }

            // Kiểm tra độ dài mật khẩu
            if (password.Length < 6 || password.Length > 100)
            {
                throw new ArgumentException("Mật khẩu phải có từ 6 đến 100 ký tự.");
            }

            // Cập nhật thông tin User
            user.UserName = username;
            user.Email = email;
            user.Password = password;

            await _context.SaveChangesAsync();

            return new RequestUserDTO
            {
                UserId = user.UserId,
                Username = user.UserName,
                Email = user.Email,
                CreatedDate = user.CreatedDate,
                Status = user.Status // Thêm Status vào DTO
            };
        }


        public async Task<bool> PermanentlyDeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false; // Không tìm thấy người dùng
            }

            // Xóa người dùng vĩnh viễn bất kể status là 0 hay 1
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> UpdateUserStatusAsync(int id, int status)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false; // Người dùng không tồn tại
            }

            user.Status = status;  // Cập nhật trạng thái người dùng
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<RequestUserDTO>> GetUsersDeleteHistoryAsync(
            string? search = null,
            string sortBy = "id",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 5)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 5;

            var query = _context.Users
                .Where(u => u.Status == 0) // Lọc người dùng đã bị xóa
                .AsQueryable();

            // Lọc theo tên người dùng (Username) nếu có search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => EF.Functions.Like(u.UserName, $"%{search}%"));
            }

            // Sắp xếp theo các trường
            switch (sortBy?.ToLower())
            {
                case "name":
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserName)
                        : query.OrderBy(u => u.UserName);
                    break;
                case "email":
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Email)
                        : query.OrderBy(u => u.Email);
                    break;
                case "id":
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserId)
                        : query.OrderBy(u => u.UserId);
                    break;
                default:
                    query = sortDirection.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.UserId)
                        : query.OrderBy(u => u.UserId);
                    break;
            }

            var totalRecords = await query.CountAsync();
            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users.Select(u => new RequestUserDTO
            {
                UserId = u.UserId,
                Username = u.UserName,
                Email = u.Email,
                CreatedDate = u.CreatedDate,
                Status = u.Status // Có thể bỏ Status nếu không cần
            }).ToList();
        }



        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<string?> LoginUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            return _jwtService.GenerateToken(user);
        }
    }
}