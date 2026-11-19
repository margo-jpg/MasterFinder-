using MasterFinder.Infrastructure;
using MasterFinder.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MasterFinder.WebHost.Helpers;

public static class MigrationManager
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        return host;
    }
}