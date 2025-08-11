using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Commands;

public record CreateProfileCommand(ProfileDto ProfileDto) : IRequest<ProfileResponseDto>;

internal sealed class CreateProfileCommandHandler(IRepository repository, IAnalyzer analyzer)
    : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    private readonly DbSet<Profile> _profiles = repository.GetDbSet();

    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        Profile profile = request.ProfileDto;
        _profiles.Add(profile);

        await repository.SaveChangesAsync(cancellationToken);

        var processedNameDto = analyzer.Process(profile.FirstName, profile.LastName);

        return new ProfileResponseDto(profile, processedNameDto);
    }
}