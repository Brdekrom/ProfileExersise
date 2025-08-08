using Microsoft.EntityFrameworkCore;
using ProfileExercise.Domain.Abstractions;

namespace ProfileExercise.Application.Abstractions;

public interface IRepository<T> where T : Entity<Guid>
{
    DbSet<T> GetDbSet();
    Task<int> SaveChangesAsync();
}