using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Movie.RequestDTO
{
    public class RequestSeriesDTO
    {
        public int SeriesId { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "ID đạo diễn là bắt buộc")]
        public int? DirectorId { get; set; }  // Nullable nếu có thể là null

        public string? Nation { get; set; }

        [Range(0, 10, ErrorMessage = "Điểm đánh giá phải từ 0 đến 10")]
        public decimal? Rating { get; set; }

        public string? PosterUrl { get; set; }

        public string? AvatarUrl { get; set; }

        public string? LinkFilmUrl { get; set; }

        //public string? Professional { get; set; }

        public string? Director { get; set; } = string.Empty!;

        public bool? IsHot { get; set; }

        [Range(1900, 2100, ErrorMessage = "Năm phát hành phải từ 1900 đến hiện tại")]
        public int? YearReleased { get; set; }

        public string? ActorsIds { get; set; }

        public string? CategoriesIds { get; set; }

        public int? Status { get; set; } // 1: active, 0: deactivated

        [Range(1, int.MaxValue, ErrorMessage = "Mùa phải là số dương")]
        public int? Season { get; set; }

        //public IFormFile PosterFile { get; set; }  // Thuộc tính nhận tệp Poster

        //public IFormFile AvatarFile { get; set; }  // Thuộc tính nhận tệp Avatar 

        public int? TotalEpisode { get; set; }

        public virtual ICollection<RequestEpisodeDTO> Episode { get; set; } = new List<RequestEpisodeDTO>();

        public virtual ICollection<RequestActorDTO> Actors { get; set; } = new List<RequestActorDTO>();

        public virtual ICollection<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();

        //public List<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();

        //public List<RequestActorDTO> Actors { get; set; } = new List<RequestActorDTO> { };

        //public List<RequestEpisodeDTO> Episode { get; set; } = new List<RequestEpisodeDTO>();
    }
}
public class ActorDTO
{
    public int ActorId { get; set; }

    public string NameAct { get; set; }
}