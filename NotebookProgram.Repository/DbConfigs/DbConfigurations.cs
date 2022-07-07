using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotebookProgram.Repository.DbConfigs
{
    public class DbConfigurations : IDbConfigurations
    {
        public string ConnectionString { get; set; }
        public DbContextOptions Options { get; set; }

        public DbConfigurations(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("myDb1");
            Options = new DbContextOptionsBuilder().UseSqlServer(ConnectionString).Options;
        }
    }
}
