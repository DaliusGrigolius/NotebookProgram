using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookProgram.Business.Interfaces;

namespace NotebookProgram.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost("Add new category")]
        public IActionResult AddNewCategory(string categoryName)
        {
            return Ok(_service.AddCategory(categoryName));
        }

        [HttpPut("Edit category name")]
        public IActionResult EditCategoryName(Guid id, string newCategoryName)
        {
            return Ok(_service.EditCategory(id, newCategoryName));
        }

        [HttpDelete("Remove category")]
        public IActionResult DeleteCategory(Guid id)
        {
            return Ok(_service.RemoveCategory(id));
        }
    }
}
