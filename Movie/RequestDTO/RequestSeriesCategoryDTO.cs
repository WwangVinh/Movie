namespace Movie.RequestDTO
{
    public class RequestSeriesCategoryDTO
    {
        public int SeriesId { get; set; }
        public int CategoryId { get; set; }

        public required RequestSeriesDTO Series { get; set; }
        public required RequestCategoryDTO Category { get; set; }
    }
}
