using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Domain.Entities;
using ProfileExercise.Persistence.Contexts;

namespace ProfileExercise.Persistence.Repositories;

public class CosmosRepository(ProfileDbContext context) : IRepository
{
    public DbSet<Profile> GetDbSet()
        => context.Set<Profile>();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}