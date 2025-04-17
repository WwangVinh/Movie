using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Movie.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Movie.RequestDTO;
using System;

namespace Movie.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly movieDB _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public ActorRepository(movieDB context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Lấy tất cả các actor với phân trang, tìm kiếm, và sắp xếp
        public async Task<IEnumerable<RequestActorDTO>> GetActorsAsync(
            string? search = null,          // Tìm kiếm theo tên hoặc mô tả actor
            string sortBy = "ActorId",      // Sắp xếp theo tên actor mặc định
            string sortDirection = "asc",   // Hướng sắp xếp mặc định là tăng dần
            int page = 1,                   // Số trang mặc định là trang 1
            int pageSize = 10                // Số lượng actor trên mỗi trang
        )
        {
            var query = _context.Actors.AsQueryable();

            // Tìm kiếm theo tên hoặc mô tả actor
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.NameAct.Contains(search) || a.Nationality.Contains(search) || a.Description.Contains(search));
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "actorid" => query.OrderByDescending(a => a.ActorId),
                    "nameact" => query.OrderByDescending(a => a.NameAct),
                    "nationality" => query.OrderByDescending(a => a.Nationality),
                    _ => query.OrderByDescending(a => a.ActorId),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "actorid" => query.OrderBy(a => a.ActorId),
                    "nameact" => query.OrderBy(a => a.NameAct),
                    "nationality" => query.OrderBy(a => a.Nationality),
                    _ => query.OrderBy(a => a.ActorId),
                };
            }

            // Phân trang
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            // Chuyển đổi từ Actor sang RequestActorDTO
            var actors = await query.Select(a => new RequestActorDTO
            {
                ActorId = a.ActorId,
                NameAct = a.NameAct,
                Description = a.Description,
                Nationality = a.Nationality,
                AvatarUrl = a.AvatarUrl
            }).ToListAsync();

            return actors;
        }

        // Lấy actor theo ID
        public async Task<RequestActorDTO?> AdminGetActorByIdAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return null;

            // Map Actor to RequestActorDTO
            return new RequestActorDTO
            {
                ActorId = actor.ActorId,
                NameAct = actor.NameAct,
                Description = actor.Description,
                Nationality = actor.Nationality,
                AvatarUrl = actor.AvatarUrl
            };
        }


        // Lưu ảnh vào thư mục chỉ định
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            _environment.WebRootPath = "C:\\Users\\Admin\\source\\repos\\Movie\\Movie\\Assets\\";
            if (file == null) return null;

            var folderPath = Path.Combine(_environment.WebRootPath, "Directors", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Lưu đường dẫn  
            return $" https://source.cmcglobal.com.vn/g1/du1.33/be-base/-/raw/main/Assets/{folderName}/{fileName}";
        }

        // Thêm mới một diễn viên
        public async Task<RequestActorDTO> AddActorAsync(RequestActorDTO actorDTO, IFormFile? AvatarUrlFile)
        {
            var avatarUrl = AvatarUrlFile != null ? await SaveFileAsync(AvatarUrlFile, "AvatarUrl") : null;

            var actor = new Models.Actor
            {
                NameAct = actorDTO.NameAct,
                Description = actorDTO.Description,
                Nationality = actorDTO.Nationality,
                AvatarUrl = avatarUrl,
                Professional = actorDTO.Professional
            };

            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            // Gán lại ID và AvatarUrl sau khi lưu
            actorDTO.ActorId = actor.ActorId;
            actorDTO.AvatarUrl = avatarUrl;

            return actorDTO;
        }

        // Sửa thông tin diễn viên
        public async Task<RequestActorDTO?> UpdateActorAsync(int id, RequestActorDTO actorDTO, IFormFile? AvatarUrlFile)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return null;

            // Nếu có ảnh mới -> upload ảnh và cập nhật
            if (AvatarUrlFile != null)
            {
                var avatarUrl = await SaveFileAsync(AvatarUrlFile, "AvatarUrl");
                actor.AvatarUrl = avatarUrl;
                actorDTO.AvatarUrl = avatarUrl;
            }

            // Cập nhật các thông tin khác
            actor.NameAct = actorDTO.NameAct;
            actor.Description = actorDTO.Description;
            actor.Nationality = actorDTO.Nationality;
            actor.Professional = actorDTO.Professional;


            await _context.SaveChangesAsync();

            actorDTO.ActorId = actor.ActorId;
            return actorDTO;
        }



        // Xóa actor theo ID
        public async Task<bool> DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return false;

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return true;
        }

        // Phương thức lưu file
        public async Task<string> SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;  // Không có file nào
            }

            // Đảm bảo thư mục lưu trữ tệp tồn tại
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);  // Tạo thư mục nếu chưa tồn tại
            }

            // Tạo tên tệp duy nhất để tránh ghi đè (ví dụ: thêm GUID vào tên tệp)
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(folderPath, fileName);

            // Lưu tệp vào hệ thống
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);  // Lưu file
            }

            return filePath;  // Trả về đường dẫn tệp đã lưu
        }

        // Lấy thông tin diễn viên và các phim liên quan
        public async Task<ActorDetailDTO?> GetActorByIdAsync(int id)
        {
            var actor = await _context.Actors
                .Include(a => a.MovieActor)
                    .ThenInclude(ma => ma.Movie)
                .Include(a => a.SeriesActors)
                    .ThenInclude(ma => ma.Series)
                .FirstOrDefaultAsync(a => a.ActorId == id);

            if (actor == null) return null;

            var actorDetail = new ActorDetailDTO
            {
                Actor = new RequestActorDTO
                {
                    ActorId = actor.ActorId,
                    NameAct = actor.NameAct,
                    Nationality = actor.Nationality
                },
                Movies = actor.MovieActor.Select(ma => new ActorMoviesDTO
                {
                    MovieId = ma.Movie.MovieId,
                    AvatarUrl = ma.Movie.AvatarUrl,
                    MovieName = ma.Movie.Title
                }).ToList(),
                Series = actor.SeriesActors.Select(ma => new ActorSeriesDTO
                {
                    SeriesId = ma.Series.SeriesId,
                    AvatarUrl = ma.Series.AvatarUrl,
                    SerieName = ma.Series.Title
                }).ToList()
            };

            return actorDetail;
        }


    }
}