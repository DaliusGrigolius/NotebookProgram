using NotebookProgram.Business.Interfaces;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.NoteServices
{
    public class NoteService : INoteService
    {
        private readonly NotebookDbContext _context;

        public NoteService(NotebookDbContext context)
        {
            _context = context;
        }

        public string CreateANote(string caption, string content)
        {
            var newNote = new Note(caption, content);
            _context.Notes.Add(newNote);
            _context.SaveChanges();

            return "Success: note created.";
        }

        public string AddImageToTheNote(Guid noteId, string imagePath)
        {
            var note = _context.Notes.Find(noteId);
            var buffer = ConvertImageToBinary(imagePath);
            var image = new Image(buffer);

            note.Images.Add(image);
            _context.Images.Add(image);
            _context.SaveChanges();

            return "Success: image added.";
        }

        private byte[] ConvertImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }

        public string AssignACategoryToTheNote(Guid noteId, Guid categoryId)
        {
            var note = _context.Notes.Find(noteId);
            var category = _context.Categories.Find(categoryId);

            category.Notes.Add(note);
            note.Categories.Add(category);
            _context.SaveChanges();

            return "Success: category assigned.";
        }

        public string EditTheNote(Guid noteId, string noteCaption, string noteContent)
        {
            var note = _context.Notes.Find(noteId);
            note.Title = noteCaption;
            note.Content = noteContent;

            _context.Notes.Update(note);
            _context.SaveChanges();

            return "Success: note updated.";
        }

        public string RemoveTheNote(Guid noteId)
        {
            var note = _context.Notes.Find(noteId);
            note.Categories.Clear();
            note.Images.Clear();
            _context.Notes.Remove(note);
            _context.SaveChanges();

            return "Success: note removed.";
        }

        public List<Note> FindNotesByTitle(string noteTitle)
        {
            var notes = _context.Notes.Where(note => note.Title == noteTitle);

            return (List<Note>)notes;
        }

        public List<Note> FilterNotesByCategoryName(string categoryName)
        {
            var categories = _context.Categories.Where(c => c.Name == categoryName);
            List<Note> notes = new();
            foreach (var category in categories)
            {
                category.Notes.ForEach(note => notes.Add(note));
            }
            return notes;
        }
    }
}
