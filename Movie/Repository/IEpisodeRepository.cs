namespace Movie.Repository
{
    public interface IEpisodeRepository<T>
    {
        Task AddAsync(T entity);

        Task DeleteBySeriesIdAsync(int seriesId);
    }
}
