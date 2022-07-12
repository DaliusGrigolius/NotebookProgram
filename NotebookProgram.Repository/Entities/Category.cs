using NotebookProgram.Repository.DbContexts;
using System.ComponentModel.DataAnnotations;

namespace NotebookProgram.Repository.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsPublic { get; set; } = false;
        public List<Note> Notes { get; set; }

        public Category(string name, bool isPublic = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            IsPublic = isPublic;
            Notes = new List<Note>();
        }

        public string AddCategory(NotebookDbContext context, string categoryName, bool isPublic = false)
        {
            context.Categories.Add(new Category(categoryName, isPublic));
            context.SaveChanges();
            return "Success: new category added.";
        }

        public string EditCategory(NotebookDbContext context, Guid categoryId, string newCategoryName)
        {
            var category = context.Categories.Find(categoryId);
            category.Name = newCategoryName;
            context.Update(category);
            context.SaveChanges();

            return "Success: category updated.";
        }

        public string RemoveCategory(NotebookDbContext context, Guid categoryId)
        {
            var category = context.Categories.Find(categoryId);
            context.Categories.Remove(category);
            context.SaveChanges();

            return "Success: category removed.";
        }
    }
}
