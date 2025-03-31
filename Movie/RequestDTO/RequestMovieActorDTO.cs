namespace Movie.RequestDTO
{
    public class RequestMovieActorDTO
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public required RequestMovieDTO Movie { get; set; }
        public required RequestActorDTO Actor { get; set; }
    }
}
