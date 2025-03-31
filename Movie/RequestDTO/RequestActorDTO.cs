using Movie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Movie.RequestDTO;

public partial class RequestActorDTO
{

    public int ActorId { get; set; }

    public string NameAct { get; set; } = null!;

    public string? Description { get; set; }

    public string? Nationality { get; set; }

    public string? Professional { get; set; }

    public string? AvatarUrl { get; set; }

    public int Status { get; set; }

    public virtual ICollection<RequestMovieDTO> Movie { get; set; } = new List<RequestMovieDTO>();

    public virtual ICollection<Series> Series { get; set; } = new List<Series>();

}

public partial class ActorMovieDTO
{
    public int MovieId { get; set; }
    public string? AvatarUrl { get; set; }
    public required string MovieName { get; set; }
}

public class ActorDetailDTO
{
    public required RequestActorDTO Actor { get; set; }
    public required List<ActorMovieDTO> Movie { get; set; }
    public required List<ActorMovieDTO> Series { get; set; }
}