using Microsoft.Extensions.Hosting;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Persistence.Contexts;
using ProfileExercise.Persistence.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class Startup
{
    public static IHostApplicationBuilder AddAspirePersistenceIntegration(this IHostApplicationBuilder builder)
    {
        builder.AddCosmosDbContext<ProfileDbContext>(connectionName: "exercisedb", "exercisedb");
        return builder;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IRepository, CosmosRepository>();
        return services;
    }
}