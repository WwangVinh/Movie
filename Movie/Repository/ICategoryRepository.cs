using Movie.RequestDTO;

namespace Movie.Repository
{
    public interface ICategoryRepository
    {
        Task<RequestCategoryDTO> CreateCategoryAsync(string categoryName);

        Task<IEnumerable<RequestCategoryDTO>> GetAllCategoriesAsync(
            string? search = null,
            string sortBy = "CategoryId",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 5);

        Task<RequestCategoryDTO> GetCategoryByIdAsync(int id);

        Task<RequestCategoryDTO> UpdateCategoryAsync(int id, string categoryName);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
