using System.Security.Claims;
using FinanceControl.DTOs;
using FinanceControl.Models;
using FinanceControl.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceControl.Controllers
{
    [ApiController]
    [Route("api/categories")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        private int GetUserId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync(GetUserId());
            var response = categories.Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Type = c.Type
            });
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                UserId = GetUserId()
            };

            var created = await _categoryRepository.CreateAsync(category);

            return CreatedAtAction(nameof(GetAll), new CategoryResponseDTO
            {
                Id = created.Id,
                Name = created.Name,
                Type = created.Type
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Id = id,
                Name = dto.Name,
                Type = dto.Type,
                UserId = GetUserId()
            };

            var updated = await _categoryRepository.UpdateAsync(category);

            if (updated == null)
                return NotFound(new { message = "Category not found." });

            return Ok(new CategoryResponseDTO
            {
                Id = updated.Id,
                Name = updated.Name,
                Type = updated.Type
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryRepository.DeleteAsync(id, GetUserId());

            if (!deleted)
                return NotFound(new { message = "Category not found." });

            return NoContent();
        }
    }
}
