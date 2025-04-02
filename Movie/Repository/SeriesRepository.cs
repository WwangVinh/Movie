using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Repository
{
    public class SeriesRepository : ISeriesRepository
    {
        private readonly movieDB _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets");

        public SeriesRepository(movieDB context, IWebHostEnvironment environment)
        {
            _environment = environment;
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
        public async Task<RequestSeriesDTO?> AdminGetSeriesByIdAsync(int id)
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

        // Lưu ảnh vào thư mục chỉ định
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            _environment.WebRootPath = "C:\\Users\\Admin\\source\\repos\\Movie\\Movie\\Assets\\";
            if (file == null) return null;

            var folderPath = Path.Combine(_environment.WebRootPath, "Assets", folderName);
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

            return $"https://source.cmcglobal.com.vn/g1/du1.33/be-base/-/raw/main/Assets/{folderName}/{fileName}";
        }


        // Thêm một bộ series mới
        public async Task<RequestSeriesDTO> AddSeriesAsync(RequestSeriesDTO seriesDTO, IFormFile posterFile, IFormFile avatarUrlFile)
        {
            var posterUrl = await SaveFileAsync(posterFile, "Posters");
            var avatarUrl = await SaveFileAsync(avatarUrlFile, "AvatarUrls");

            var series = new Series
            {
                Title = seriesDTO.Title,
                Description = seriesDTO.Description,
                DirectorId = seriesDTO.DirectorId,
                Rating = seriesDTO.Rating,
                IsHot = seriesDTO.IsHot,
                YearReleased = seriesDTO.YearReleased,
                PosterUrl = posterUrl,  // Lưu đường dẫn poster
                AvatarUrl = avatarUrl,  // Lưu đường dẫn avatar
                Status = seriesDTO.Status,
                Nation = seriesDTO.Nation,
                Season = seriesDTO.Season
            };

            _context.Series.Add(series);
            await _context.SaveChangesAsync();

            seriesDTO.PosterUrl = posterUrl;
            seriesDTO.AvatarUrl = avatarUrl;

            //Xử lý thêm Category vào MovieCategory
            if (seriesDTO.CategoriesIds != null && seriesDTO.CategoriesIds.Any())
            {
                string[] CategoriesId = seriesDTO.CategoriesIds.Split(',');

                foreach (var category in CategoriesId)
                {
                    _context.SeriesCategories.Add(new SeriesCategories
                    {
                        SeriesId = series.SeriesId,
                        CategoryId = Int32.Parse(category)
                    });
                }
            }

            //  Xử lý thêm Actor vào MovieActor
            if (seriesDTO.ActorsIds != null && seriesDTO.ActorsIds.Any())
            {
                string[] ActorsId = seriesDTO.ActorsIds.Split(',');

                foreach (var actor in ActorsId)
                {
                    _context.SeriesActors.Add(new SeriesActors
                    {
                        SeriesActorId = series.SeriesId,
                        ActorId = Int32.Parse(actor)
                    });
                }
            }

            await _context.SaveChangesAsync();

            seriesDTO.PosterUrl = posterUrl;
            seriesDTO.AvatarUrl = avatarUrl;

            return seriesDTO;
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

        public async Task<RequestSeriesDTO> GetSeriesByIdAsync(int id)
        {

            var series = await _context.Series
            .Include(s => s.SeriesActors)
                .ThenInclude(sa => sa.Actors)
            .Include(s => s.SeriesCategories)
                .ThenInclude(sc => sc.Categories)
            .Include(s => s.Director)
            .FirstOrDefaultAsync(s => s.SeriesId == id);

            if (series == null) return null;

            var seriesDTO = new RequestSeriesDTO
            {
                Title = series.Title,
                YearReleased = series.YearReleased,
                Nation = series.Nation ?? string.Empty,
                Categories = series.SeriesCategories.Select(sa => new RequestCategoryDTO
                {
                    CategoryId = sa.Categories.CategoryId,
                    CategoryName = sa.Categories.CategoryName
                }).ToList(),

                Description = series.Description ?? string.Empty,
                Episode = await _context.Episodes
                    .Where(e => e.SeriesId == series.SeriesId)
                    .Select(e => new RequestEpisodeDTO
                    {
                        EpisodeNumber = e.EpisodeNumber,
                        Title = e.Title ?? string.Empty,
                        LinkFilmUrl = e.LinkFilmUrl ?? string.Empty
                    }).ToListAsync(),
                TotalEpisode = series.Status ?? 0,
                Actors = series.SeriesActors.Select(sa => new RequestActorDTO
                {
                    ActorId = sa.ActorId,
                    NameAct = sa.Actors.NameAct
                }).ToList(),
                Director = series.Director?.NameDir ?? string.Empty
            };

            return seriesDTO;
        }

        public async Task<IEnumerable<RequestSeriesDTO>> GetSeriesAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID)
        {
            var query = _context.Series
                .Where(s => s.Status == 1);

            // Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Title.Contains(search));
            }

            if (categoryID.HasValue)
            {
                query = query.Where(s => s.SeriesCategories.Any(sc => sc.CategoryId == categoryID.Value));
            }

            // Sorting
            query = sortBy switch
            {
                "Title" => query.OrderBy(s => s.Title),
                "Rating" => query.OrderByDescending(s => s.Rating),
                _ => query.OrderBy(s => s.Title)
            };

            var seriesList = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return seriesList.Select(s => new RequestSeriesDTO
            {
                SeriesId = s.SeriesId,
                Title = s.Title,
                PosterUrl = s.PosterUrl,
                AvatarUrl = s.AvatarUrl,


            }).ToList();
        }

    }
}
