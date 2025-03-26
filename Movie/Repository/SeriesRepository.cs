using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Repository
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly movieDB _context;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public SeriesRepository(movieDB context)
        {
            _context = context;
        }

        // Lấy danh sách series với phân trang, tìm kiếm và sắp xếp
        public async Task<PaginatedList<RequestSeriesDTO>> GetSeriesAsync(
            string? search = null,
            string sortBy = "Title",          // Sắp xếp theo tên series mặc định
            string sortDirection = "asc",     // Hướng sắp xếp mặc định là tăng dần
            int page = 1,                    // Số trang
            int pageSize = 5                 // Số lượng series trên mỗi trang
        )
        {
            var query = _context.Series.AsQueryable();

            // Tìm kiếm theo tiêu đề
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Title.Contains(search));
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "rating" => query.OrderByDescending(s => s.Rating),
                    "yearreleased" => query.OrderByDescending(s => s.YearReleased),
                    _ => query.OrderByDescending(s => s.Title),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "rating" => query.OrderBy(s => s.Rating),
                    "yearreleased" => query.OrderBy(s => s.YearReleased),
                    _ => query.OrderBy(s => s.Title),
                };
            }

            // Lấy tổng số bản ghi để tính phân trang
            var totalRecords = await query.CountAsync();

            // Phân trang
            var series = await query
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(s => new RequestSeriesDTO
                                {
                                    SeriesId = s.SeriesId,
                                    Title = s.Title,
                                    Description = s.Description,
                                    DirectorId = s.DirectorId,
                                    Rating = s.Rating,
                                    IsHot = s.IsHot ?? false, // Xử lý nullable
                                    YearReleased = s.YearReleased,
                                    PosterUrl = s.PosterUrl,
                                    AvatarUrl = s.AvatarUrl,
                                    Status = s.Status ?? 0, // Xử lý nullable
                                    Season = s.Season,
                                    LinkFilmUrl = s.LinkFilmUrl,  // Thêm LinkFilmUrl
                                    Nation = s.Nation  // Thêm Nation
                                })
                                .ToListAsync();

            // Return Paginated List
            return new PaginatedList<RequestSeriesDTO>(
                series,
                totalRecords,
                page,
                pageSize
            );
        }


        // Lấy thông tin series theo ID
        public async Task<RequestSeriesDTO?> GetSeriesByIdAsync(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series == null) return null;

            return new RequestSeriesDTO
            {
                SeriesId = series.SeriesId,
                Title = series.Title,
                Description = series.Description,
                DirectorId = series.DirectorId,
                Rating = series.Rating,
                IsHot = series.IsHot ?? false, // Xử lý nullable
                YearReleased = series.YearReleased,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
                Status = series.Status ?? 0, // Xử lý nullable
                Season = series.Season
            };
        }

        public async Task<string> SaveFile(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
            {
                return null; // No file to save
            }

            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", subFolder);

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Generate a unique file name
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(directoryPath, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Return the saved file path
        }



        // Thêm một bộ series mới
        public async Task<RequestSeriesDTO> AddSeriesAsync(RequestSeriesDTO seriesDTO, string posterFilePath, string avatarFilePath)
        {
            var series = new Series
            {
                Title = seriesDTO.Title,
                Description = seriesDTO.Description,
                DirectorId = seriesDTO.DirectorId,
                Rating = seriesDTO.Rating,
                IsHot = seriesDTO.IsHot,
                YearReleased = seriesDTO.YearReleased,
                PosterUrl = posterFilePath,  // Lưu đường dẫn poster
                AvatarUrl = avatarFilePath,  // Lưu đường dẫn avatar
                Status = seriesDTO.Status,
                Nation = seriesDTO.Nation,
                Season = seriesDTO.Season
            };

            _context.Series.Add(series);
            await _context.SaveChangesAsync();

            return new RequestSeriesDTO
            {
                SeriesId = series.SeriesId,
                Title = series.Title,
                Description = series.Description,
                DirectorId = series.DirectorId,
                Rating = series.Rating,
                IsHot = series.IsHot ?? false,
                YearReleased = series.YearReleased,
                PosterUrl = series.PosterUrl,
                AvatarUrl = series.AvatarUrl,
                Status = series.Status ?? 0,
                Nation = seriesDTO.Nation,
                Season = series.Season
            };
        }





        // Cập nhật thông tin của bộ series
        public async Task<RequestSeriesDTO> UpdateSeriesAsync(int id, RequestSeriesDTO seriesDTO, string? posterFilePath, string? avatarFilePath)
        {
            // Tìm kiếm series theo ID
            var existingSeries = await _context.Series.FindAsync(id);

            // Kiểm tra nếu không tìm thấy series
            if (existingSeries == null)
            {
                return null;
            }

            // Cập nhật các thông tin từ seriesDTO (chỉ những trường bạn cần)
            existingSeries.Title = seriesDTO.Title;
            existingSeries.Description = seriesDTO.Description;
            existingSeries.DirectorId = seriesDTO.DirectorId;
            existingSeries.Rating = seriesDTO.Rating;
            existingSeries.IsHot = seriesDTO.IsHot;
            existingSeries.YearReleased = seriesDTO.YearReleased;
            existingSeries.Season = seriesDTO.Season;

            // Cập nhật đường dẫn poster nếu có
            if (posterFilePath != null)
            {
                existingSeries.PosterUrl = posterFilePath;
            }

            // Cập nhật đường dẫn avatar nếu có
            if (avatarFilePath != null)
            {
                existingSeries.AvatarUrl = avatarFilePath;
            }

            // Cập nhật các trường khác nếu cần thiết, ví dụ: LinkFilmUrl, Nation
            existingSeries.LinkFilmUrl = seriesDTO.LinkFilmUrl;
            existingSeries.Nation = seriesDTO.Nation;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Series.Update(existingSeries);
            await _context.SaveChangesAsync();

            // Trả về DTO của series đã được cập nhật
            return new RequestSeriesDTO
            {
                SeriesId = existingSeries.SeriesId,
                Title = existingSeries.Title,
                Description = existingSeries.Description,
                DirectorId = existingSeries.DirectorId,
                Rating = existingSeries.Rating,
                IsHot = existingSeries.IsHot,
                YearReleased = existingSeries.YearReleased,
                Season = existingSeries.Season,
                PosterUrl = existingSeries.PosterUrl,
                AvatarUrl = existingSeries.AvatarUrl,
                Status = existingSeries.Status ?? 0,  // Ensure nullable fields are handled
                LinkFilmUrl = existingSeries.LinkFilmUrl,
                Nation = existingSeries.Nation
            };
        }





        // Xóa một bộ series hoàn toàn khỏi cơ sở dữ liệu
        public async Task DeleteSeriesAsync(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                _context.Series.Remove(series);
                await _context.SaveChangesAsync();
            }
        }

        // Xóa mềm một bộ series (đặt Status = 0)
        public async Task SoftDeleteSeriesAsync(int id)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                series.Status = 0;  // Đặt Status = 0 khi xóa mềm
                _context.Series.Update(series);
                await _context.SaveChangesAsync();
            }
        }

        // Cập nhật trạng thái của series
        public async Task UpdateSeriesStatusAsync(int id, int status)
        {
            var series = await _context.Series.FindAsync(id);
            if (series != null)
            {
                series.Status = status;
                _context.Series.Update(series);
                await _context.SaveChangesAsync();
            }
        }
    }
}
