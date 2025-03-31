using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Movie.RequestDTO
{
    public class RequestSeriesDTO
    {
        public int SeriesId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? DirectorId { get; set; }  // Nullable nếu có thể là null

        [StringLength(255)]
        public string? Professional { get; set; }

        public string? PosterUrl { get; set; }

        [StringLength(255)]
        public string? AvatarUrl { get; set; }

        public int? Status { get; set; } // 1: active, 0: deactivated

        public string? LinkFilmUrl { get; set; }

        [StringLength(255)]
        public string? Nation { get; set; }

        public bool? IsHot { get; set; }

        public int? YearReleased { get; set; }

        public int? Season { get; set; }

        public decimal? Rating { get; set; }

        public string? ActorsIds { get; set; } // Có thể là chuỗi ID của các diễn viên

        public string? CategoriesIds { get; set; } // Có thể là chuỗi ID của các thể loại

        //public DateTime? DeletedAt { get; set; } // Lưu thời gian xóa nếu cần

        //public DateTime? RestoredAt { get; set; } // Lưu thời gian khôi phục nếu cần

        // Các thuộc tính nhận tệp từ form
        [Required(ErrorMessage = "Poster file is required")]
        public IFormFile PosterFile { get; set; }  // Tệp ảnh Poster

        [Required(ErrorMessage = "Avatar file is required")]
        public IFormFile AvatarFile { get; set; }  // Tệp ảnh Avatar
        public string? Director { get; set; } = string.Empty!;
        public int? TotalEpisode { get; set; }
        public virtual ICollection<RequestEpisodeDTO> Episode { get; set; } = new List<RequestEpisodeDTO>();
        public virtual ICollection<RequestActorDTO> Actors { get; set; } = new List<RequestActorDTO>();
        public virtual ICollection<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();

    }
}
public class ActorDTO
{
    public int ActorId { get; set; }
    public string NameAct { get; set; }
}