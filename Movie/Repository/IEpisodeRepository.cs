using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IEpisodeRepository
    {
        //Task AddAsync(Episode episode); // Thêm tập phim
        Task<RequestEpisodeDTO?> AddEpisodeAsync(RequestEpisodeDTO episodeDTO);
        Task<Episode> GetByIdAsync(int episodeId); // Lấy tập phim theo ID
        Task<List<Episode>> GetBySeriesIdAsync(int seriesId); // Lấy tất cả tập phim của series
        Task UpdateAsync(Episode episode); // Cập nhật tập phim
        Task DeleteAsync(int episodeId); // Xóa tập phim
        Task DeleteBySeriesIdAsync(int seriesId); // Xóa tất cả episode theo SeriesId
    }
}