using Core;
using Data.DataAccess;
using Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Settings;

public static class DependencyInjection
{
    public static void AddDatabase(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=atm.db"))
            .AddTransient<IAtmRepository, AtmRepository>();
    }
    
    public static void InitializeDatabase(this IServiceProvider serviceProvider)
    {
        var context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
    }
}