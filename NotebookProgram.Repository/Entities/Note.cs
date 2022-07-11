using NotebookProgram.Repository.DbContexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public List<Image> Images { get; set; }
        public List<Category> Categories { get; set; }

        public Note(string title, string content, bool isPublic = false)
        {
            Id = Guid.NewGuid();
            Title = title;
            Content = content;
            IsPublic = isPublic;
            Categories = new List<Category>();
            Images = new List<Image>();
        }

        public string CreateANote(NotebookDbContext context, string caption, string content, bool isPublic = false)
        {
            var newNote = new Note(caption, content, isPublic);
            context.Notes.Add(newNote);
            context.SaveChanges();

            return "Success: note created.";
        }

        public string AddImageToTheNote(NotebookDbContext context, Guid noteId, string imagePath)
        {
            var note = context.Notes.Find(noteId);
            var buffer = ConvertImageToBinary(imagePath);
            var image = new Image(buffer);

            note.Images.Add(image);
            context.Images.Add(image);
            context.SaveChanges();

            return "Success: image added.";
        }

        public string AssignACategoryToTheNote(NotebookDbContext context, Guid noteId, Guid categoryId)
        {
            var note = context.Notes.Find(noteId);
            var category = context.Categories.Find(categoryId);

            category.Notes.Add(note);
            note.Categories.Add(category);
            context.SaveChanges();

            return "Success: category assigned.";
        }

        public string EditTheNote(NotebookDbContext context, Guid noteId, string noteCaption, string noteContent)
        {
            var note = context.Notes.Find(noteId);
            note.Title = noteCaption;
            note.Content = noteContent;

            context.Notes.Update(note);
            context.SaveChanges();

            return "Success: note updated.";
        }

        public string RemoveTheNote(NotebookDbContext context, Guid noteId)
        {
            var note = context.Notes.Find(noteId);
            note.Categories.Clear();
            note.Images.Clear();
            context.Notes.Remove(note);
            context.SaveChanges();

            return "Success: note removed.";
        }

        public List<Note> FindNotesByTitle(NotebookDbContext context, string noteTitle)
        {
            var notes = context.Notes.Where(note => note.Title == noteTitle);

            return (List<Note>)notes;
        }

        public List<Note> FilterNotesByCategoryName(NotebookDbContext context, string categoryName)
        {
            var categories = context.Categories.Where(c => c.Name == categoryName);
            List<Note> notes = new();
            foreach (var category in categories)
            {
                category.Notes.ForEach(note => notes.Add(note));
            }
            return notes;
        }

        private byte[] ConvertImageToBinary(string imagePath)
        {
            FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer, 0, (int)fileStream.Length);
            fileStream.Close();
            return buffer;
        }
    }
}
