namespace NotebookProgram.Dto.Models
{
    public class NoteDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public List<ImageDto> Images { get; set; }
        public List<CategoryDto> Categories { get; set; }

        public NoteDto(Guid id, string title, string content, Guid userId)
        {
            Id = id;
            Title = title;
            Content = content;
            UserId = userId;
            Images = new List<ImageDto>();
            Categories = new List<CategoryDto>();
        }
    }
}
