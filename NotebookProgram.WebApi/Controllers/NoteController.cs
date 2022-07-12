using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookProgram.Business.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotebookProgram.WebApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;

        public NoteController(INoteService service)
        {
            _service = service;
        }

        [HttpPost("Create a note")]
        public IActionResult CreateNote(string title, string content)
        {
            return Ok(_service.CreateANote(title, content));
        }

        [HttpPut("Edit the note")]
        public IActionResult EditNote(Guid noteId, string newTitle, string newContent)
        {
            return Ok(_service.EditTheNote(noteId, newTitle, newContent));
        }

        [HttpPut("Assign category to the note")]
        public IActionResult AssignCategory(Guid noteId, Guid categoryId)
        {
            return Ok(_service.AssignACategoryToTheNote(noteId, categoryId));
        }

        [HttpPut("Add image to the note")]
        public IActionResult AddImage(Guid noteId, string filePath)
        {
            return Ok(_service.AddImageToTheNote(noteId, filePath));
        }

        [HttpDelete("Remove the note")]
        public IActionResult DeleteNote(Guid noteId)
        {
            return Ok(_service.RemoveTheNote(noteId));
        }

        [HttpGet("Find notes by title")]
        public IActionResult FindNotes(string noteTitle)
        {
            return Ok(_service.FindNotesByTitle(noteTitle));
        }

        [HttpGet("Filter notes by category")]
        public IActionResult FilterNotes(string categoryName)
        {
            return Ok(_service.FilterNotesByCategoryName(categoryName));
        }
    }
}
