using NotebookProgram.Business.Interfaces;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly NotebookDbContext _context;
        private readonly IUserService _service;

        public CategoryService(NotebookDbContext context, IUserService service)
        {
            _context = context;
            _service = service;
        }

        public string AddCategory(string categoryName)
        {
            var userId = _service.GetCurrentUserId();
            var category = new Category(categoryName);
            category.UserId = (Guid)userId;

            _context?.Categories?.Add(category);
            _context?.SaveChanges();

            return "Success: new category added.";
        }

        public string EditCategory(Guid categoryId, string newCategoryName)
        {
            var userId = _service.GetCurrentUserId();
            var currentCategory = _context.Categories
                .Where(c => c.Id == categoryId && c.UserId == userId)
                .FirstOrDefault();

            if (currentCategory != null)
            {
                currentCategory.Name = newCategoryName;
                _context.SaveChanges();
            }

            _context.SaveChanges();

            return "Success: category updated.";
        }

        public List<Category> GetallCategories()
        {
            var userId = _service.GetCurrentUserId();
            var getAllCategories = _context?.Categories?.Where(i => i.UserId == userId).ToList();

            return getAllCategories;
        }

        public string RemoveCategory(Guid categoryId)
        {
            var category = _context.Categories.Find(categoryId);
            _context.Categories.Remove(category);
            _context.SaveChanges();

            return "Success: category removed.";
        }
    }
}
