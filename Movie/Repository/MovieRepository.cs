using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Movie.Models;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{

    public class MovieRepository : IMovieRepository
    {
        private readonly movieDB _context;
        private readonly IWebHostEnvironment _environment;
        public MovieRepository(movieDB context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<RequestMovieDTO?> GetByIdAsync(int id)
        {

            var movie = await _context.Movies
            .Include(b => b.MovieActor)
                .ThenInclude(sa => sa.Actors)
            .Include(b => b.MovieCategories)
                .ThenInclude(sc => sc.Categories)
            .Include(b => b.Director)
            .FirstOrDefaultAsync(b => b.MovieId == id);

            if (movie == null) return null;
            return new RequestMovieDTO
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                Rating = movie.Rating,
                PosterUrl = movie.PosterUrl,
                AvatarUrl = movie.AvatarUrl,
                LinkFilmUrl = movie.LinkFilmUrl,
                IsHot = movie.IsHot,
                YearReleased = movie.YearReleased,
                Categories = movie.MovieCategories
                    .Select(sc => new RequestCategoryDTO
                    {
                        CategoryName = sc.Categories.CategoryName
                    }).ToList(),
                Actors = movie.MovieActor.Select(sa => new RequestActorDTO
                {
                    ActorId = sa.ActorId,
                    NameAct = sa.Actors.NameAct
                }).ToList(),
                Director = movie.Director?.NameDir ?? string.Empty
            };
        }

        // Lưu ảnh vào thư mục chỉ định
        private async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            _environment.WebRootPath = "C:\\Users\\Admin\\OneDrive\\Tài liệu\\movieDB\\Movies\\Assets\\";
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

            // Lưu đường dẫn  
            return $" https://source.cmcglobal.com.vn/g1/du1.33/be-base/-/raw/main/Assets/{folderName}/{fileName}";
        }

        //  Thêm phim
        public async Task<RequestMovieDTO> AddAsync(RequestMovieDTO movieDTO, IFormFile posterFile, IFormFile AvatarUrlFile)
        {
            var posterUrl = await SaveFileAsync(posterFile, "Posters");
            var AvatarUrl = await SaveFileAsync(AvatarUrlFile, "AvatarUrl");

            var movie = new Models.Movies
            {
                Title = movieDTO.Title,
                Description = movieDTO.Description,
                Rating = movieDTO.Rating,
                PosterUrl = posterUrl,
                AvatarUrl = AvatarUrl,
                LinkFilmUrl = movieDTO.LinkFilmUrl,
                Nation = movieDTO.Nation,
                DirectorId = movieDTO.DirectorId,
                IsHot = movieDTO.IsHot,
                YearReleased = movieDTO.YearReleased,
                Status = 1
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            movieDTO.PosterUrl = posterUrl;
            movieDTO.AvatarUrl = AvatarUrl;



            //Xử lý thêm Category vào MovieCategory
            if (movieDTO.CategoryIds != null && movieDTO.CategoryIds.Any())
            {
                string[] categoryId = movieDTO.CategoryIds.Split(',');

                foreach (var category in categoryId)
                {
                    _context.MovieCategories.Add(new MovieCategories
                    {
                        MovieId = movie.MovieId,
                        CategoryId = Int32.Parse(category)
                    });
                }
            }

            //  Xử lý thêm Actor vào MovieActors
            if (movieDTO.ActorIds != null && movieDTO.ActorIds.Any())
            {
                string[] actorId = movieDTO.ActorIds.Split(',');

                foreach (var actor in actorId)
                {
                    _context.MovieActor.Add(new MovieActors
                    {
                        MovieId = movie.MovieId,
                        ActorId = Int32.Parse(actor)
                    });
                }
            }

            await _context.SaveChangesAsync();

            movieDTO.PosterUrl = posterUrl;
            movieDTO.AvatarUrl = AvatarUrl;

            return movieDTO;
        }




        public async Task<IEnumerable<RequestMovieDTO>> GetMovieAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID)
        {
            var query = _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActor).ThenInclude(ma => ma.Actors)
                .Include(m => m.MovieCategories).ThenInclude(mc => mc.Categories)
                .Where(m => m.Status == 1);

            //  Filtering
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Title.Contains(search));
            }

            if (categoryID.HasValue)
            {
                query = query.Where(m => m.MovieCategories.Any(mc => mc.CategoryId == categoryID.Value));
            }

            //  Sorting
            query = sortBy switch
            {
                "Title" => query.OrderBy(m => m.Title),
                "Rating" => query.OrderByDescending(m => m.Rating),
                _ => query.OrderBy(m => m.Title)
            };

            var Movie = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Movie.Select(m => new RequestMovieDTO
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                Rating = m.Rating,
                IsHot = m.IsHot,
                Nation = m.Nation,
                YearReleased = m.YearReleased,
                PosterUrl = m.PosterUrl,
                AvatarUrl = m.AvatarUrl,
                LinkFilmUrl = m.LinkFilmUrl,
                DirectorId = m.DirectorId,
                Director = m.Director?.NameDir,
                Actors = m.MovieActor.Select(ma => new RequestActorDTO
                {
                    ActorId = ma.Actors.ActorId,
                    NameAct = ma.Actors.NameAct
                }).ToList(),
                Categories = m.MovieCategories.Select(mc => new RequestCategoryDTO
                {
                    CategoryId = mc.Categories.CategoryId,
                    CategoryName = mc.Categories.CategoryName
                }).ToList()
            }).ToList();
        }
        //Xoá mềm 
        public async Task<RequestMovieDTO?> SoftDeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                movie.Status = 0;
                await _context.SaveChangesAsync();
            }
            return null;
        }


        public async Task<RequestMovieDTO> GetMovieByIdAsync(int id)
        {

            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActor)
                    .ThenInclude(ma => ma.Actors)
                .Include(m => m.MovieCategories)
                    .ThenInclude(mc => mc.Categories)
                .FirstOrDefaultAsync(m => m.MovieId == id);

            if (movie == null) return null!;

            var movieDTO = new RequestMovieDTO
            {
                Title = movie.Title,
                LinkFilmUrl = movie.LinkFilmUrl,
                YearReleased = movie.YearReleased,
                Nation = movie.Nation,
                Categories = movie.MovieCategories
                    .Select(mc => new RequestCategoryDTO
                    {
                        CategoryName = mc.Categories.CategoryName
                    }).ToList(),
                Description = movie.Description,
                Actors = movie.MovieActor.Select(ma => new RequestActorDTO
                {
                    ActorId = ma.ActorId,
                    NameAct = ma.Actors.NameAct
                }).ToList(),
                Director = movie.Director!.NameDir
            };

            return movieDTO;
        }
    }
}