using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Tests.Helpers;

internal sealed class FakeRepository(TestDbContext db) : IRepository
{
    public DbSet<Profile> GetDbSet() => db.Set<Profile>();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => db.SaveChangesAsync(cancellationToken);
}