using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MP.Core.Context;

namespace MP.Core.Npgsql
{
    class DesignContextFactory : IDesignTimeDbContextFactory<MPContext>
    {
        public MPContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MPContext>();
            optionsBuilder.UseNpgsql("Host=127.0.0.1;Database=mp", b => b.MigrationsAssembly("MP.Core.Npgsql"));

            return new MPContext(optionsBuilder.Options);
        }
    }
}
