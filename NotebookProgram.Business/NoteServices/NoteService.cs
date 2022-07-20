using Microsoft.EntityFrameworkCore;
using NotebookProgram.Business.Interfaces;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.NoteServices
{
    public class NoteService : INoteService
    {
        private readonly NotebookDbContext _context;
        private readonly IUserService _service;

        public NoteService(NotebookDbContext context, IUserService service)
        {
            _context = context;
            _service = service;
        }

        public string CreateANote(string caption, string content)
        {
            var newNote = new Note(caption, content);
            var currentUser = _service.GetCurrentUser();
            if(currentUser == null)
            {
                return "Error: user not found.";
            }

            currentUser.Notes.Add(newNote);
            _context?.Notes?.Add(newNote);
            _context?.SaveChanges();

            return "Success: note created.";
        }

        public string AddImageToTheNote(Guid noteId, byte[] img)
        {
            var note = _context?.Notes?.Find(noteId);

            if (note == null)
            {
                return "Error: note not found.";
            }

            var image = new Image(img);

            note.Images.Add(image);
            _context?.Images?.Add(image);
            _context?.SaveChanges();

            return "Success: image added.";
        }

        public string AssignACategoryToTheNote(Guid noteId, Guid categoryId)
        {
            var note = _context?.Notes?
                .Include(i => i.Categories)
                .FirstOrDefault(i => i.Id == noteId);

            if (note == null)
            {
                return "Error: note not found.";
            }

            if (note.Categories.Any(i => i.Id == categoryId))
            {
                return "Category already assigned.";
            }

            var category = _context?.Categories?.Find(categoryId);
            if (category == null)
            {
                return "Error: category not found.";
            }

            category?.Notes.Add(note);
            note?.Categories.Add(category);
            _context?.SaveChanges();

            return "Success: category assigned.";
        }

        public string EditTheNote(Guid noteId, string noteCaption, string noteContent)
        {
            var note = _context?.Notes?.Find(noteId);
            if (note == null)
            {
                return "Error: note not found.";
            }

            note.Title = noteCaption;
            note.Content = noteContent;

            _context?.Notes?.Update(note);
            _context?.SaveChanges();

            return "Success: note updated.";
        }

        public string RemoveTheNote(Guid noteId)
        {
            var note = _context?.Notes?.Find(noteId);
            if (note == null)
            {
                return "Error: note not found.";
            }

            note.Categories.Clear();
            note.Images.Clear();
            _context?.Notes?.Remove(note);
            _context?.SaveChanges();

            return "Success: note removed.";
        }

        public List<Note> FindNotesByTitle(string noteTitle)
        {
            var notes = _context?.Notes?
                .Include(i => i.Images)
                .Include(i => i.Categories)
                .Where(note => note.Title == noteTitle).ToList();

            return notes;
        }

        public List<Note> FindNotesByCategoryName(string categoryName)
        {
            var filteredNotes = _context?.Notes?
                .Include(i => i.Images)
                .Include(i => i.Categories)
                .Where(i => i.Categories.Any(i => i.Name == categoryName)).ToList();

            return filteredNotes;
        }

        public List<Note> GetAllNotesByUser()
        {
            var userId = _service.GetCurrentUserId();

            var notes = _context?.Notes?
                .Include(i => i.Images)
                .Include(i => i.Categories)
                .Where(i => i.UserId == userId)
                .ToList();

            return notes;
        }
    }
}
