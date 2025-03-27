using Movie.RequestDTO;

namespace Movie.ResponseDTO
{
    public class SeriesResponseDTO
    {

        public string NameSeries { get; set; } = string.Empty;
        public string SeriesLink { get; set; } = string.Empty;
        public int? YearReleased { get; set; }
        public string National { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Episode { get; set; }
        public int? TotalEpisode { get; set; }
        public required string ActorName { get; set; }
        public List<RequestActorDTO> Actors { get; set; } = new();
        public string Director { get; set; } = string.Empty;
    }
    public class ResponseEpisodeDTO
    {
        public int EpisodeNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string LinkFilmURL { get; set; } = string.Empty;
    }
}
