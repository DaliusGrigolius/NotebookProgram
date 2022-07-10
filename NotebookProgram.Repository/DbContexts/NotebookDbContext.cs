using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NotebookProgram.Repository.DbConfigs;
using NotebookProgram.Repository.Entities;

namespace NotebookProgram.Repository.DbContexts
{
    public class NotebookDbContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Note>? Notes { get; set; }
        public DbSet<Image>? Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer($"Server=localhost;Database=NotebookDB;Trusted_Connection=True;");

        //public NotebookDbContext(IDbConfigurations options) : base(options.Options)
        //{

        //}
    }
}
