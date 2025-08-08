using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Application.Services;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Queries;

public record GetProfileByIdQuery(Guid ProfileId) : IRequest<ProfileResponseDto>;

internal sealed class GetProfileByIdQueryHandler(IRepository<Profile> profileRepo, INameService nameService)
    : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    private readonly DbSet<Profile> _profiles = profileRepo.GetDbSet();

    public Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = _profiles.FirstOrDefault(p => p.Id == request.ProfileId);

        if (profile == null)
            throw new KeyNotFoundException(
                $"Profile with Id '{request.ProfileId}' not found.");

        var processed = nameService.Process(profile.FirstName, profile.LastName);

        var dto = new ProfileResponseDto(
            profile,
            processed);

        return Task.FromResult(dto);
    }
}