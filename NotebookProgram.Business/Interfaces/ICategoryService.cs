using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.Interfaces
{
    public interface ICategoryService
    {
        string AddCategory(string categoryName);
        string EditCategory(Guid categoryId, string newCategoryName);
        string RemoveCategory(Guid categoryId);
        List<Category> GetallCategories();
    }
}