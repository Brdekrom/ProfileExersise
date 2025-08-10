using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Commands;

public record DeleteProfileCommand(Guid Id) : IRequest<bool>;

internal sealed class DeleteProfileCommandHandler(IRepository repository)
    : IRequestHandler<DeleteProfileCommand, bool>
{
    private readonly DbSet<Profile> _profiles = repository.GetDbSet();

    public async Task<bool> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _profiles.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (profile is null)
            throw new NullReferenceException("Profile not found");

        _profiles.Remove(profile);
        await repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}