using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Queries;

public record GetProfileByIdQuery(Guid ProfileId) : IRequest<ProfileResponseDto>;

internal sealed class GetProfileByIdQueryHandler(IRepository profileRepo, IAnalyzer analyzer)
    : IRequestHandler<GetProfileByIdQuery, ProfileResponseDto>
{
    private readonly DbSet<Profile> _profiles = profileRepo.GetDbSet();

    public async Task<ProfileResponseDto> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        var profile = await _profiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.ProfileId, cancellationToken: cancellationToken);

        if (profile == null)
            throw new KeyNotFoundException(
                $"Profile with Id '{request.ProfileId}' not found.");

        var processed = analyzer.Process(profile.FirstName, profile.LastName);

        var dto = new ProfileResponseDto(
            profile,
            processed);

        return dto;
    }
}