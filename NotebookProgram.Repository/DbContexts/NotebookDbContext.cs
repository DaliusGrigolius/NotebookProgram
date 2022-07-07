using Microsoft.EntityFrameworkCore;
using NotebookProgram.Repository.DbConfigs;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Repository.DbContexts
{
    public class NotebookDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UsersDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Note> Notes { get; set; }

        public NotebookDbContext(IDbConfigurations options) : base(options.Options)
        {

        }
    }
}
