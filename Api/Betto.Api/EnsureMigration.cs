using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Betto.Api
{
    public static class EnsureMigration
    {
        public static void EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            var context = app.ApplicationServices.GetService<T>();
            context.Database.Migrate();
        }
    }
}