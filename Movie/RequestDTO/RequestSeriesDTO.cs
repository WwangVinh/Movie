using System;
using System.ComponentModel.DataAnnotations;

namespace Movie.Models
{
    public class RequestSeriesDTO
    {
        public int SeriesId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? DirectorId { get; set; }  // Thay đổi thành nullable nếu có thể là null

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

        public DateTime? YearReleased { get; set; }

        public int? Season { get; set; }

        public decimal? Rating { get; set; }

        public DateTime? DeletedAt { get; set; } // Nếu series bị xóa, bạn có thể lưu thời gian xóa

        public DateTime? RestoredAt { get; set; } // Nếu series được khôi phục, bạn có thể lưu thời gian khôi phục
    }
}
