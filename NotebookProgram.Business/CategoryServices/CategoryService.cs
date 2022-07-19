using NotebookProgram.Business.Interfaces;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly NotebookDbContext _context;

        public CategoryService(NotebookDbContext context)
        {
            _context = context;
        }

        public string AddCategory(string categoryName)
        {
            _context?.Categories?.Add(new Category(categoryName));
            _context?.SaveChanges();
            return "Success: new category added.";
        }

        public string EditCategory(Guid categoryId, string newCategoryName)
        {
            var category = _context.Categories.Find(categoryId);
            category.Name = newCategoryName;
            _context.Update(category);
            _context.SaveChanges();

            return "Success: category updated.";
        }

        public List<Category> GetallCategories()
        {
            return _context.Categories.ToList();
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
