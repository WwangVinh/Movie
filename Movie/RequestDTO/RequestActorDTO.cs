using Movie.Models;
namespace Movie.RequestDTO;

public partial class RequestActorDTO
{

    public int ActorId { get; set; }

    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    public string? Nationality { get; set; }

    public string? Professional { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual ICollection<RequestMovieDTO> Movie { get; set; } = new List<RequestMovieDTO>();

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();

}

public partial class ActorMoviesDTO
{
    public int MovieId { get; set; }
    public string? AvatarUrl { get; set; }
    public required string MovieName { get; set; }
}

public partial class ActorSeriesDTO
{
    public int SeriesId { get; set; }
    public string? AvatarUrl { get; set; }
    public required string SerieName { get; set; }
}


public class ActorDetailDTO
{
    public required RequestActorDTO Actor { get; set; }
    public required List<ActorMoviesDTO> Movies { get; set; }
    public required List<ActorSeriesDTO> Series { get; set; }
}