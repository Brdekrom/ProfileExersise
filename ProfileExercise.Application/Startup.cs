using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IAnalyzer, FullNameProcessor>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        return services;
    }
}