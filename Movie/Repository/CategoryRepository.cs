using Movie.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Movie.RequestDTO;

namespace Movie.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly movieDB _context;

        public CategoryRepository(movieDB context)
        {
            _context = context;
        }

        // Tạo mới một thể loại phim
        public async Task<RequestCategoryDTO> CreateCategoryAsync(string categoryName)
        {
            var existingCategory = await _context.Categories
                                                 .FirstOrDefaultAsync(c => c.CategoryName == categoryName);
            if (existingCategory != null)
            {
                return null;  // Nếu thể loại phim đã tồn tại, trả về null
            }

            var newCategory = new Categories { CategoryName = categoryName };
            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return new RequestCategoryDTO
            {
                CategoryId = newCategory.CategoryId,
                CategoryName = newCategory.CategoryName
            };
        }

        // Lấy tất cả thể loại phim với phân trang, tìm kiếm, và sắp xếp
        public async Task<IEnumerable<RequestCategoryDTO>> GetAllCategoriesAsync(
            string? search = null,
            string sortBy = "CategoryId",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Categories.AsQueryable();

            // Lọc theo tên thể loại nếu có search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.CategoryName.Contains(search));
            }

            // Sắp xếp theo sortBy và sortDirection
            if (sortDirection.ToLower() == "desc")
            {
                query = sortBy.ToLower() switch
                {
                    "categoryid" => query.OrderByDescending(c => c.CategoryId),
                    "categoryname" => query.OrderByDescending(c => c.CategoryName),
                    _ => query.OrderByDescending(c => c.CategoryId),
                };
            }
            else
            {
                query = sortBy.ToLower() switch
                {
                    "categoryid" => query.OrderBy(c => c.CategoryId),
                    "categoryname" => query.OrderBy(c => c.CategoryName),
                    _ => query.OrderBy(c => c.CategoryId),
                };
            }

            // Phân trang
            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return await query.Select(c => new RequestCategoryDTO
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToListAsync();
        }

        // Lấy thể loại phim theo ID
        public async Task<RequestCategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                                          .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return null;
            }

            return new RequestCategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        // Cập nhật thông tin thể loại phim
        public async Task<RequestCategoryDTO> UpdateCategoryAsync(int id, string categoryName)
        {
            var category = await _context.Categories
                                          .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return null;
            }

            category.CategoryName = categoryName;
            await _context.SaveChangesAsync();

            return new RequestCategoryDTO
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
        }

        // Xóa thể loại phim
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.MovieCategories
                                          .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null)
            {
                return false;
            }

            _context.MovieCategories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
