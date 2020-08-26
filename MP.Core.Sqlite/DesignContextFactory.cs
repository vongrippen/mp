using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MP.Core.Context;

namespace MP.Core.Sqlite
{
    class DesignContextFactory : IDesignTimeDbContextFactory<MPContext>
    {
        public MPContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MPContext>();
            optionsBuilder.UseSqlite("Data Source=database.db", b => b.MigrationsAssembly("MP.Core.Sqlite"));

            return new MPContext(optionsBuilder.Options);
        }
    }
}
