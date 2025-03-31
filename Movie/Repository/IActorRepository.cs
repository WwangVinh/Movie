using Movie.Models;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface IActorRepository
    {
        Task<IEnumerable<RequestActorDTO>> GetActorsAsync(
        string? search = null,           // Tìm kiếm theo tên hoặc mô tả actor
        string sortBy = "ActorId",       // Sắp xếp theo tên actor mặc định
        string sortDirection = "asc",    // Hướng sắp xếp mặc định là tăng dần
        int page = 1,                    // Số trang mặc định là trang 1
        int pageSize = 10                 // Số lượng actor trên mỗi trang
        );
        Task<RequestActorDTO?> AdminGetActorByIdAsync(int id);
        Task<RequestActorDTO?> AddActorAsync(RequestActorDTO actor);
        Task<RequestActorDTO?> UpdateActorAsync(int id, RequestActorDTO actor);
        Task DeleteActorAsync(int id);
        Task<string> SaveFile(IFormFile file, string folderName);  // Phương thức mới
        Task<ActorDetailDTO?> GetActorByIdAsync(int id);
    }
}
