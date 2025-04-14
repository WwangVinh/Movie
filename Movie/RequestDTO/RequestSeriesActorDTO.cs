namespace Movie.RequestDTO
{
    public class RequestSeriesActorDTO
    {
        public int SeriesId { get; set; }
        public int ActorId { get; set; }
        public required RequestSeriesDTO Series { get; set; }
        public required RequestActorDTO Actor { get; set; }
    }
}
