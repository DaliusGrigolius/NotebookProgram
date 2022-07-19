using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookProgram.Business.Interfaces;
using NotebookProgram.Dto.Models;
using NotebookProgram.Repository.Entities;

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

        [HttpPost("Create-a-note")]
        public async Task<IActionResult> CreateNote(string title, string content)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.CreateANote(title, content);
            });

            return Ok(result);
        }

        [HttpGet("Show-all-notes")]
        public async Task<IActionResult> ShowNotes()
        {
            var notes = new List<Note>();

            await Task.Run(() =>
            {
                notes = _service.GetAllNotesByUser();
            });

            if (notes.Count == 0)
            {
                return NotFound("Notes not found");
            }

            TransferDataToNoteDto(notes, out List<NoteDto> noteDtoList);

            return Ok(noteDtoList);
        }

        [HttpGet("Find-notes-by-title")]
        public async Task<IActionResult> FilterNotesByTitle(string noteTitle)
        {
            var filteredNotes = new List<Note>();

            await Task.Run(() =>
            {
                filteredNotes = _service.FindNotesByTitle(noteTitle);
            });

            if (filteredNotes.Count == 0)
            {
                return NotFound("Notes not found");
            }

            TransferDataToNoteDto(filteredNotes, out List<NoteDto> noteDtoList);

            return Ok(noteDtoList);
        }

        [HttpGet("Find-notes-by-category-name")]
        public async Task<IActionResult> FilterNotesByCategoryName(string categoryName)
        {
            var filteredNotes = new List<Note>();

            await Task.Run(() =>
            {
                filteredNotes = _service.FindNotesByCategoryName(categoryName);
            });

            if (filteredNotes.Count == 0)
            {
                return NotFound("Notes not found");
            }

            TransferDataToNoteDto(filteredNotes, out List<NoteDto> noteDtoList);

            return Ok(noteDtoList);
        }

        [HttpPut("Edit-the-note")]
        public async Task<IActionResult> EditNote(Guid noteId, string newTitle, string newContent)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.EditTheNote(noteId, newTitle, newContent);
            });

            return Ok(result);
        }

        [HttpPut("Assign-category-to-the-note")]
        public async Task<IActionResult> AssignCategory(Guid noteId, Guid categoryId)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.AssignACategoryToTheNote(noteId, categoryId);
            });

            return Ok(result);
        }

        [HttpPost("Add-image-to-the-note")]
        public async Task<IActionResult> AddImage(Guid noteId, IFormFile file)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.AddImageToTheNote(noteId, file); // ON DUTY
            });

            return Ok(result);
        }

        [HttpDelete("Remove-the-note")]
        public async Task<IActionResult> DeleteNote(Guid noteId)
        {
            string result = "";
            await Task.Run(() =>
            {
                result = _service.RemoveTheNote(noteId);
            });

            return Ok(result);
        }

        private void TransferDataToNoteDto(List<Note> filteredNotes, out List<NoteDto> noteDtoList)
        {
            noteDtoList = new List<NoteDto>();

            for (int i = 0; i < filteredNotes.Count; i++)
            {
                noteDtoList.Add(new NoteDto(filteredNotes[i].Id, filteredNotes[i].Title, filteredNotes[i].Content, filteredNotes[i].UserId));

                if (filteredNotes[i].Categories.Count != 0)
                {
                    for (int j = 0; j < filteredNotes[i].Categories.Count; j++)
                    {
                        noteDtoList[i].Categories.Add(new CategoryDto
                        {
                            Id = filteredNotes[i].Categories[j].Id,
                            Name = filteredNotes[i].Categories[j].Name
                        });
                    }
                }

                if (filteredNotes[i].Images.Count != 0)
                {
                    for (int j = 0; j < filteredNotes[i].Images.Count; j++)
                    {
                        //string result = System.Text.Encoding.UTF8.GetString(filteredNotes[i].Images[j].Byte);
                        
                        noteDtoList[i].Images.Add(new ImageDto
                        {
                            Id = filteredNotes[i].Images[j].Id,
                            Byte = filteredNotes[i].Images[j].Byte,
                        });
                    }
                }
            }
        }
    }
}
