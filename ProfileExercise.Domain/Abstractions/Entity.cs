namespace ProfileExercise.Domain.Abstractions;

public abstract class Entity<TId> where TId : unmanaged
{
    public TId Id { get; init; }
}