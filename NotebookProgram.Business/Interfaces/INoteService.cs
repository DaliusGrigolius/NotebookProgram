using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.Interfaces
{
    public interface INoteService
    {
        string AddImageToTheNote(NotebookDbContext context, Guid noteId, string imagePath);
        string AssignACategoryToTheNote(NotebookDbContext context, Guid noteId, Guid categoryId);
        string CreateANote(NotebookDbContext context, string caption, string content);
        string EditTheNote(NotebookDbContext context, Guid noteId, string noteCaption, string noteContent);
        List<Note> FilterNotesByCategoryName(NotebookDbContext context, string categoryName);
        List<Note> FindNotesByTitle(NotebookDbContext context, string noteTitle);
        string RemoveTheNote(NotebookDbContext context, Guid noteId);
    }
}