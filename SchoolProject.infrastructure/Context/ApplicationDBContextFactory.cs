using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SchoolProject.Infrastructure.Data
{
    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            // Build configuration
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../SchoolProject.Api");
            if (!Directory.Exists(basePath))
            {
                // Try alternative path (when running from solution root)
                basePath = Path.Combine(Directory.GetCurrentDirectory(), "SchoolProject.Api");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            // Build options
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            var connectionString = configuration.GetConnectionString("dbcontext");
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDBContext(optionsBuilder.Options);
        }
    }
}

