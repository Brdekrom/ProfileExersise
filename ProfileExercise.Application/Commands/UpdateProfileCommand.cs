using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Commands;

public record UpdateProfileCommand(ProfileDto ProfileDto) : IRequest<ProfileResponseDto>;

internal sealed class UpdateProfileCommandHandler(IRepository repository, IAnalyzer analyzer)
    : IRequestHandler<UpdateProfileCommand, ProfileResponseDto>
{
    private readonly DbSet<Profile> _profiles = repository.GetDbSet();

    public async Task<ProfileResponseDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var id = Guid.Parse(request.ProfileDto.Id);
        var profile = await _profiles
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (profile == null)
            throw new NullReferenceException("Profile not found");

        profile.Update(request.ProfileDto);

        await repository.SaveChangesAsync(cancellationToken);

        var processedNameDto = analyzer.Process(profile.FirstName, profile.LastName);

        return new ProfileResponseDto(profile, processedNameDto);
    }
}