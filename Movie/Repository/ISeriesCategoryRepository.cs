namespace Movie.Repository
{
    public interface ISeriesCategoryRepository<SeriesCategory>
    {
        Task AddAsync(SeriesCategory entity);

        Task DeleteBySeriesIdAsync(int id);
    }
}