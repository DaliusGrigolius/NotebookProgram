using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotebookProgram.Business.Interfaces;
using NotebookProgram.Repository.DbContexts;
using NotebookProgram.Repository.Entities;
using System.Security.Claims;

namespace NotebookProgram.Business.UserServices
{
    public class UserService : IUserService
    {
        private readonly NotebookDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public UserService(NotebookDbContext context, IHttpContextAccessor httpContext)
        {
            _context = context;
            _httpContext = httpContext;
        }

        public User? GetCurrentUser()
        {
            Guid? userId = GetCurrentUserId();
            if (userId != null)
            {
                var user = _context?.Users?
                    .Include(i => i.Notes)
                    .FirstOrDefault(u => u.Id == userId);

                return user;
            }
            return null;
        }

        public Guid? GetCurrentUserId()
        {
            string? userId = _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return null;
            }

            if (Guid.TryParse(userId, out Guid userIdParsed))
            {
                return userIdParsed;
            }
            else
            {
                return null;
            }
        }
    }
}
