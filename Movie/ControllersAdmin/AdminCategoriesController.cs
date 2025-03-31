using Microsoft.AspNetCore.Mvc;
using Movie.Repository;
using Movie.RequestDTO;
using Movie.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movie.ControllersAdmin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public AdminCategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<RequestMovieCategoryDTO>> CreateMovieCategory(string categoryName)
        {
            try
            {
                var createdCategory = await _categoryRepository.CreateCategoryAsync(categoryName);

                if (createdCategory == null)
                {
                    return BadRequest(new { Message = "Tên thể loại phim đã tồn tại." });
                }

                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.CategoryId }, createdCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestMovieCategoryDTO>>> GetCategories(
            string? search = null,
            string sortBy = "CategoryId",
            string sortDirection = "asc",
            int page = 1,
            int pageSize = 10)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(search, sortBy, sortDirection, page, pageSize);
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestMovieCategoryDTO>> GetCategoryById(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { Message = "Thể loại phim không tìm thấy." });
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieCategory(int id, string categoryName)
        {
            try
            {
                var updatedCategory = await _categoryRepository.UpdateCategoryAsync(id, categoryName);

                if (updatedCategory == null)
                {
                    return NotFound(new { Message = "Không tìm thấy thể loại phim." });
                }

                return Ok(updatedCategory);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { Message = "Không tìm thấy thể loại phim." });
            }

            var result = await _categoryRepository.DeleteCategoryAsync(id);

            if (!result)
            {
                return BadRequest(new { Message = "Xóa thể loại phim không thành công." });
            }

            return Ok(new { Message = "Thể loại phim đã được xóa thành công." });
        }
    }
}
