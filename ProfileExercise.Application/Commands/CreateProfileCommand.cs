using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Application.Services;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Commands;

public record CreateProfileCommand(ProfileDto ProfileDto) : IRequest<ProfileResponseDto>;

internal sealed class CreateProfileCommandHandler(IRepository<Profile> repository, INameService nameService)
    : IRequestHandler<CreateProfileCommand, ProfileResponseDto>
{
    private readonly DbSet<Profile> _profiles = repository.GetDbSet();

    public async Task<ProfileResponseDto> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        Profile profile = request.ProfileDto;
        _profiles.Add(profile);

        await repository.SaveChangesAsync();

        var processedNameDto = nameService.Process(profile.FirstName, profile.LastName);

        return new ProfileResponseDto(profile, processedNameDto);
    }
}