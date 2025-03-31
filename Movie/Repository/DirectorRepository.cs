using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Repository
{
    public class DirectorRepository : IDirectorsRepository
    {
        private readonly movieDB _context;

        public DirectorRepository(movieDB context)
        {
            _context = context;
        }

        // Lấy chi tiết đạo diễn theo ID, bao gồm các bộ phim và series mà đạo diễn tham gia
        public async Task<DirectorDetailDTO?> GetDirectorByIdAsync(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movie)  // Bao gồm các bộ phim mà đạo diễn tham gia
                .Include(d => d.Series) // Bao gồm các series mà đạo diễn tham gia
                .FirstOrDefaultAsync(d => d.DirectorId == id);  // Lấy đạo diễn theo ID

            if (director == null) return null;

            var directorDetail = new DirectorDetailDTO
            {
                Director = new RequestDirectorDTO
                {
                    DirectorID = director.DirectorId,
                    NameDir = director.NameDir,
                    Nationality = director.Nationality,
                    AvatarUrl = director.AvatarUrl,
                    Professional = director.Professional
                },
                Movies = director.Movie.Select(m => new DirectorMoviesDTO
                {
                    MovieId = m.MovieId,
                    AvatarUrl = m.AvatarUrl,
                    MovieName = m.Title
                }).ToList(),
                Series = director.Series.Select(s => new DirectorMoviesDTO
                {
                    MovieId = s.SeriesId,
                    AvatarUrl = s.AvatarUrl,
                    MovieName = s.Title
                }).ToList()
            };

            return directorDetail;
        }



        // Lấy tất cả các Director với phân trang, tìm kiếm, và sắp xếp
        public async Task<IEnumerable<RequestDirectorDTO>> GetAllDirectorsAsync(
            string? search = null,
            string sortBy = "DirectorId",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Directors.AsQueryable();

            // Tìm kiếm theo tên hoặc mô tả director
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.NameDir.Contains(search) || d.Nationality.Contains(search) || d.Description.Contains(search));
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "directorid" => query.OrderByDescending(d => d.DirectorId),
                    "namedir" => query.OrderByDescending(d => d.NameDir),
                    "nationality" => query.OrderByDescending(d => d.Nationality),
                    _ => query.OrderByDescending(d => d.DirectorId),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "directorid" => query.OrderBy(d => d.DirectorId),
                    "namedir" => query.OrderBy(d => d.NameDir),
                    "nationality" => query.OrderBy(d => d.Nationality),
                    _ => query.OrderBy(d => d.DirectorId),
                };
            }

            // Phân trang
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            // Chuyển đổi thành RequestDirectorDTO
            var directorDTOs = await query
                .Select(d => new RequestDirectorDTO
                {
                    DirectorID = d.DirectorId,
                    NameDir = d.NameDir,
                    Nationality = d.Nationality,
                    AvatarUrl = d.AvatarUrl,
                    Description = d.Description
                })
                .ToListAsync();

            return directorDTOs;
        }

        // Thêm mới một đạo diễn
        public async Task<RequestDirectorDTO> AddDirectorAsync(RequestDirectorDTO directorDTO)
        {
            var director = new Director
            {
                NameDir = directorDTO.NameDir,
                Nationality = directorDTO.Nationality,
                AvatarUrl = directorDTO.AvatarUrl,
                Professional = directorDTO.Professional
            };

            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            return new RequestDirectorDTO
            {
                DirectorID = director.DirectorId,
                NameDir = director.NameDir,
                Nationality = director.Nationality,
                AvatarUrl = director.AvatarUrl,
                Professional = director.Professional
            };
        }

        // Cập nhật thông tin đạo diễn
        public async Task<RequestDirectorDTO> UpdateDirectorAsync(int id, RequestDirectorDTO directorDTO)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null) return null;

            director.NameDir = directorDTO.NameDir;
            director.Nationality = directorDTO.Nationality;
            director.AvatarUrl = directorDTO.AvatarUrl;
            director.Professional = directorDTO.Professional;

            await _context.SaveChangesAsync();

            return new RequestDirectorDTO
            {
                DirectorID = director.DirectorId,
                NameDir = director.NameDir,
                Nationality = director.Nationality,
                AvatarUrl = director.AvatarUrl,
                Professional = director.Professional
            };
        }
    }
}
