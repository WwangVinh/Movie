using System.Collections.Generic;
using System.Threading.Tasks;
using Movie.RequestDTO;
using Movie.ResponseDTO;

namespace Movie.Repository
{
    public interface IMovieHome
    {
        //Home
        Task<IEnumerable<string>> GetPostersAsync();
        Task<IEnumerable<RequestMovieDTO>> GetNewMovieAsync();
        Task<IEnumerable<RequestMovieDTO>> GetHotMovieAsync();
        Task<IEnumerable<RequestMovieDTO>> GetSeriesMovieAsync();
        Task<IEnumerable<RequestMovieDTO>> GetActionMovieAsync();
    }
}
