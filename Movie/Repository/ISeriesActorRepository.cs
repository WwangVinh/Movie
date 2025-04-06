namespace Movie.Repository
{
    public interface ISeriesActorRepository<SeriesActor>
    {
        Task AddAsync(SeriesActor entity);

        Task DeleteBySeriesIdAsync(int id);
    }
}
