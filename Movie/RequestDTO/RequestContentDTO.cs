namespace Movie.RequestDTO
{
    public class RequestContentDTO
    {
        public class ActionContentDto
        {
            public int Id { get; set; }              // MovieId hoặc SeriesId
            public string Title { get; set; }
            public string AvatarUrl { get; set; }
            public string Type { get; set; }         // "Movie" hoặc "Series"
        }


    }
}