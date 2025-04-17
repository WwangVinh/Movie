namespace Movie.ResponseDTO
{
    public class MovieResponseDTO
    {
        public string Title { get; set; } = null!;
        public string? LinkFilmUrl { get; set; } 
        public int YearReleased { get; set; }
        //public string National { get; set; }
        public string Categories { get; set; }
        public string Description { get; set; }
        public List<ActorDTO> Actor { get; set; }
        public string Director { get; set; }
    }

    public class ActorDTO
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
    }


}
