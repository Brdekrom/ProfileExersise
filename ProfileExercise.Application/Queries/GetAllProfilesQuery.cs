using MediatR;
using Microsoft.EntityFrameworkCore;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.Entities;

namespace ProfileExercise.Application.Queries;

public record GetAllProfilesQuery : IRequest<List<ProfileResponseDto>>;

internal sealed class GetAllProfilesQueryHandler(IRepository profileRepo, IAnalyzer analyzer)
    : IRequestHandler<GetAllProfilesQuery, List<ProfileResponseDto>>
{
    private readonly DbSet<Profile> _profiles = profileRepo.GetDbSet();

    public async Task<List<ProfileResponseDto>> Handle(
        GetAllProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var profiles = await _profiles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var result = profiles
            .Select(p =>
            {
                var processedNameDto = analyzer.Process(p.FirstName, p.LastName);
                return new ProfileResponseDto(p, processedNameDto);
            })
            .ToList();

        return result;
    }
}