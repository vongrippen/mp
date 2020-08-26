using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MP.Core.Context;
using System;

namespace MP.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataServices( this IServiceCollection services, IConfiguration configuration)
        {
            var databaseProvider = configuration["DatabaseProvider"];
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<MPContext>(o =>
            {
                switch (databaseProvider)
                {
                    case "sqlite":
                            o.UseSqlite(connectionString, b => b.MigrationsAssembly("MP.Core.Sqlite"));
                        break;

                    case "sqlserver":
                            o.UseSqlServer(connectionString, b => b.MigrationsAssembly("MP.Core.SqlServer"));
                        break;

                    case "npgsql":
                            o.UseNpgsql(connectionString, b => b.MigrationsAssembly("MP.Core.Npgsql"));
                        break;

                    default:
                        throw new NotImplementedException($"Database provider: {databaseProvider} is not supported. Currently only sqlite, sqlserver, and npgsql are supported");
                }
            });



            return services;
        }
    }
}
