using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface ISeries
    {
        Task<RequestMovieDTO> GetSeriesByIdAsync(int id);
        Task<IEnumerable<Series>> GetAllAsync();
        Task<Series> GetByIdAsync(int id);
        Task AddAsync(Series entity);
        Task UpdateAsync(Series entity);
        Task DeleteAsync(int id);


    }
}
