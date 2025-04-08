
namespace Movie.RequestDTO;

public partial class RequestDirectorDTO
{

    public int DirectorID { get; set; }

    public string NameDir { get; set; } = null!;

    public string? Description { get; set; }


    public string? Nationality { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Professional { get; set; }

}
public class DirectorMoviesDTO
{
    public int MovieId { get; set; }
    public string? AvatarUrl { get; set; }
    public required string MovieName { get; set; }
}

public class DirectorDetailDTO
{
    public required RequestDirectorDTO Director { get; set; }
    public required List<DirectorMoviesDTO> Movies { get; set; }
    public required List<DirectorMoviesDTO> Series { get; set; }
}