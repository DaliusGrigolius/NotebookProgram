using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotebookProgram.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotebookController : ControllerBase
    {
        private readonly NotebookDbContext _context;

        public NotebookController(NotebookDbContext context)
        {
            _context = context;
        }

        [HttpGet("get by id"), Authorize]
        public IActionResult Get(bool visible)
        {
            return Ok(_context.Notes.Where(i => i.IsPublic == visible));
        }

        [HttpGet("{id}"), Authorize]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost("add note"), Authorize]
        public void Post([FromBody] Note note)
        {
            _context.Notes.Add(note);
        }

        [HttpPut("{id}"), Authorize]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}"), Authorize]
        public void Delete(int id)
        {
        }
    }
}
