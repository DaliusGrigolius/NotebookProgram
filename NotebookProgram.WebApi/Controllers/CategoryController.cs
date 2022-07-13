using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookProgram.Business.Interfaces;

namespace NotebookProgram.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost("Add new category")]
        public async Task<IActionResult> AddNewCategory(string categoryName)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.AddCategory(categoryName);
            });

            return Ok(result);
        }

        [HttpPut("Edit category name")]
        public async Task<IActionResult> EditCategoryName(Guid id, string newCategoryName)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.EditCategory(id, newCategoryName);
            });

            return Ok(result);
        }

        [HttpDelete("Remove category")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.RemoveCategory(id);
            });

            return Ok(result);
        }
    }
}
