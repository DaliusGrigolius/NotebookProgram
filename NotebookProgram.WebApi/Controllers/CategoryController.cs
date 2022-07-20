using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookProgram.Business.Interfaces;
using NotebookProgram.Dto.Models;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IUserService _userService;

        public CategoryController(ICategoryService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost("Add-new-category")]
        public async Task<IActionResult> AddNewCategory(string categoryName)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.AddCategory(categoryName);
            });

            return Ok(result);
        }

        [HttpGet("Get-all-categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            List<Category> categories = null;

            await Task.Run(() =>
            {
                categories = _service.GetallCategories();
            });

            if (categories.Count == 0)
            {
                return NotFound("Categories not found");
            }

            TransferDataToCategoriesDto(categories, out List<CategoryDto> categoriesDtoList);

            return Ok(categoriesDtoList);
        }

        [HttpPut("Edit-category-name")]
        public async Task<IActionResult> EditCategoryName(Guid id, string newCategoryName)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.EditCategory(id, newCategoryName);
            });

            return Ok(result);
        }

        [HttpDelete("Remove-category")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.RemoveCategory(id);
            });

            return Ok(result);
        }

        private void TransferDataToCategoriesDto(List<Category> categories, out List<CategoryDto> categoriesDtoList)
        {

            var userId = _userService.GetCurrentUserId();

            categoriesDtoList = new List<CategoryDto>();

            for (int i = 0; i < categories.Count; i++)
            {
                categoriesDtoList.Add(new CategoryDto
                {
                    Id = categories[i].Id,
                    Name = categories[i].Name,
                    UserId = (Guid)userId,
                });
            }
        }
    }
}
