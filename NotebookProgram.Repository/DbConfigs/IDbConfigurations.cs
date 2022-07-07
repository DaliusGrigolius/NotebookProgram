using Microsoft.EntityFrameworkCore;

namespace NotebookProgram.Repository.DbConfigs
{
    public interface IDbConfigurations
    {
        string ConnectionString { get; set; }
        DbContextOptions Options { get; set; }
    }
}