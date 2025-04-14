using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations; // Đừng quên import

namespace Movie.RequestDTO
{
    public class RequestEpisodeDTO
    {
        public int EpisodeId { get; set; }

        [Required(ErrorMessage = "SeriesId là bắt buộc")]
        public int SeriesId { get; set; }

        [Required(ErrorMessage = "Số tập là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "Số tập phải là số dương")]
        public int EpisodeNumber { get; set; }

        [StringLength(255, ErrorMessage = "Tiêu đề tập không được vượt quá 255 ký tự")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Link phim là bắt buộc")]
        [StringLength(255, ErrorMessage = "Link phim không được vượt quá 255 ký tự")]
        public string LinkFilmUrl { get; set; } = null!;
    }
}
