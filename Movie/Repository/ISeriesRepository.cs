using Movie.RequestDTO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.Repository
{
    public class PaginatedList<T> : IEnumerable<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public PaginatedList(List<T> items, int totalRecords, int page, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            Page = page;
            PageSize = pageSize;
        }

        // Implement the GetEnumerator method
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        // Explicit implementation of IEnumerable.GetEnumerator to comply with the interface
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }



    public interface ISeriesRepository
    {
        // Lấy danh sách series với phân trang, tìm kiếm, và sắp xếp
        Task<PaginatedList<RequestSeriesDTO>> GetSeriesAsync(
            string? search = null,         // Tìm kiếm theo tiêu đề series
            string sortBy = "Title",       // Sắp xếp theo tên series mặc định
            string sortDirection = "asc",  // Hướng sắp xếp mặc định là tăng dần
            int page = 1,                  // Số trang mặc định là trang 1
            int pageSize = 10               // Số lượng series trên mỗi trang
        );


        // Thêm một bộ series mới
        Task<RequestSeriesDTO> AddSeriesAsync(RequestSeriesDTO seriesDTO, IFormFile posterFile, IFormFile AvatarUrlFile);

        // Cập nhật thông tin của bộ series
        Task<RequestSeriesDTO> UpdateSeriesAsync(int id, RequestSeriesDTO seriesDTO, string? posterFilePath, string? avatarFilePath);

        // Xóa một bộ series hoàn toàn khỏi cơ sở dữ liệu
        Task DeleteSeriesAsync(int id);

        // Xóa mềm một bộ series (đặt Status = 0)
        Task SoftDeleteSeriesAsync(int id);

        // Cập nhật trạng thái của series (Ví dụ: có thể thay đổi giữa trạng thái 1 (hoạt động) và 0 (xóa))
        Task UpdateSeriesStatusAsync(int id, int status);

        // Thêm khai báo phương thức SaveFile
        //Task<string> SaveFile(IFormFile file, string subFolder);

        Task<RequestSeriesDTO> GetSeriesByIdAsync(int id);
        Task<IEnumerable<RequestSeriesDTO>> GetSeriesAsync(int pageNumber, int pageSize, string sortBy, string search, int? categoryID);
    }
}