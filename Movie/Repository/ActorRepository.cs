using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Movie.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly movieDB _context;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public ActorRepository(movieDB context)
        {
            _context = context;
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


        // Cập nhật thông tin actor
        public async Task<RequestActorDTO?> AddActorAsync(RequestActorDTO actorDTO)
        {
            // Kiểm tra nếu Actor với cùng tên đã tồn tại
            var existingActor = await _context.Actors
                                              .FirstOrDefaultAsync(a => a.NameAct == actorDTO.NameAct);
            if (existingActor != null)
            {
                // Nếu Actor đã tồn tại, trả về null hoặc thông báo lỗi
                return null; // Hoặc throw new ArgumentException("Actor with this name already exists.");
            }

            // Chuyển đổi từ RequestActorDTO sang Actor
            var actor = new Actor
            {
                NameAct = actorDTO.NameAct,
                Description = actorDTO.Description,
                Nationality = actorDTO.Nationality,
                AvatarUrl = actorDTO.AvatarUrl
            };

            // Thêm actor mới vào database
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            // Trả về actor vừa được thêm, nhưng phải trả về kiểu RequestActorDTO
            return new RequestActorDTO
            {
                ActorId = actor.ActorId,
                NameAct = actor.NameAct,
                Description = actor.Description,
                Nationality = actor.Nationality,
                AvatarUrl = actor.AvatarUrl
            };
        }

        public async Task<RequestActorDTO?> UpdateActorAsync(int id, RequestActorDTO actorDTO)
        {
            var existingActor = await _context.Actors.FindAsync(id);
            if (existingActor == null) return null;

            // Cập nhật các thông tin của actor từ RequestActorDTO
            existingActor.NameAct = actorDTO.NameAct;
            existingActor.Description = actorDTO.Description;
            existingActor.Nationality = actorDTO.Nationality;
            existingActor.Professional = actorDTO.Professional;
            existingActor.AvatarUrl = actorDTO.AvatarUrl;

            // Lưu thay đổi vào database
            _context.Actors.Update(existingActor);
            await _context.SaveChangesAsync();

            // Trả về actor đã được cập nhật dưới dạng RequestActorDTO
            return new RequestActorDTO
            {
                ActorId = existingActor.ActorId,
                NameAct = existingActor.NameAct,
                Description = existingActor.Description,
                Nationality = existingActor.Nationality,
                AvatarUrl = existingActor.AvatarUrl
            };
        }


        // Xóa actor theo ID
        public async Task DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor != null)
            {
                _context.Actors.Remove(actor);
                await _context.SaveChangesAsync();
            }
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
                Movie = actor.MovieActor.Select(ma => new ActorMovieDTO
                {
                    MovieId = ma.Movie.MovieId,
                    AvatarUrl = ma.Movie.AvatarUrl,
                    MovieName = ma.Movie.Title
                }).ToList(),
                Series = actor.SeriesActors.Select(ma => new ActorMovieDTO
                {
                    MovieId = ma.Series.SeriesId,
                    AvatarUrl = ma.Series.AvatarUrl,
                    MovieName = ma.Series.Title
                }).ToList()
            };

            return actorDetail;
        }


    }
}
