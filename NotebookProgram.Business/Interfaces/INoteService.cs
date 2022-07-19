using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.Interfaces
{
    public interface INoteService
    {
        string AddImageToTheNote(Guid noteId, byte[] base64image);
        string AssignACategoryToTheNote(Guid noteId, Guid categoryId);
        string CreateANote(string caption, string content);
        string EditTheNote(Guid noteId, string noteCaption, string noteContent);
        List<Note> FindNotesByCategoryName(string categoryName);
        List<Note> FindNotesByTitle(string noteTitle);
        string RemoveTheNote(Guid noteId);
        List<Note> GetAllNotesByUser();
    }
}