using Microsoft.Extensions.DependencyInjection;

namespace Core.Settings;

public static class DependencyInjection
{
    public static void AddService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IAtmService, AtmService>();
    }
}