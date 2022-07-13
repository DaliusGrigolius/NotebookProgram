using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Business.Interfaces
{
    public interface IUserService
    {
        Guid? GetCurrentUserId();
        User? GetCurrentUser();
    }
}