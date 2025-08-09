using Microsoft.EntityFrameworkCore;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Abstractions;

public interface IRepository
{
    DbSet<Profile> GetDbSet();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}