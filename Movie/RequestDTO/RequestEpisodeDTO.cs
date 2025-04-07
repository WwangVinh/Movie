
namespace Movie.RequestDTO;

public partial class RequestEpisodeDTO
{

    public int EpisodeId { get; set; }


    public int? SeriesId { get; set; }

    public int EpisodeNumber { get; set; }


    public string? EpisodeTitle { get; set; }

  
    public string LinkFilmUrl { get; set; } = null!;


}
