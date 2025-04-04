using Movie.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie.RequestDTO;

public partial class RequestMovieDTO
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? DirectorId { get; set; }

    public string? Nation { get; set; }

    public decimal? Rating { get; set; }

    public string? PosterUrl { get; set; }

    public string? AvatarUrl { get; set; }

    public string? LinkFilmUrl { get; set; }

    public string? Director { get; set; } = string.Empty!;

    public bool? IsHot { get; set; }

    public int? YearReleased { get; set; }

    public string? ActorIds { get; set; }

    public string? CategoryIds { get; set; }

    public int? Status { get; set; }

    public List<RequestCategoryDTO> Categories { get; set; } = new List<RequestCategoryDTO>();

    public List<RequestActorDTO> Actors {  get; set; } = new List<RequestActorDTO> { };
}
public class ActorDTO
{
    public int ActorId { get; set; }

    public string NameAct { get; set; }

}


