using ProfileExercise.Domain;
using ProfileExercise.Domain.Entities;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Application.DataTransferObjects;

public sealed record ProfileDto(
    string? Id,
    string FirstName,
    string LastName,
    IReadOnlyList<SocialSkillDto> SocialSkills,
    IReadOnlyList<SocialAccountDto> SocialAccounts
)
{
    public static implicit operator Profile(ProfileDto dto)
    {
        if (dto is null) throw new ArgumentNullException(nameof(dto));

        var firstNameVo = new Name(dto.FirstName);
        var lastNameVo = new Name(dto.LastName);

        var socialSkills = (dto.SocialSkills ?? Enumerable.Empty<SocialSkillDto>())
            .Select(s => new SocialSkill(s.Value));

        var socialAccounts = (dto.SocialAccounts ?? Enumerable.Empty<SocialAccountDto>())
            .Select(a => new SocialAccount(a.Type, a.Address));

        var profile = new Profile(firstNameVo, lastNameVo, socialSkills, socialAccounts);

        if (!string.IsNullOrWhiteSpace(dto.Id) && Guid.TryParse(dto.Id, out var guidId))
            profile.Id = guidId;
        else
            profile.Id = Guid.NewGuid();

        return profile;
    }


    public static implicit operator ProfileDto(Profile profile)
    {
        return new ProfileDto(
            profile.Id.ToString(),
            profile.FirstName.Value,
            profile.LastName.Value,
            profile.SocialSkills.Select(s => new SocialSkillDto(s.Value)).ToList(),
            profile.SocialAccounts.Select(a => new SocialAccountDto(a.SocialMediaType, a.Address)).ToList()
        );
    }
}

public record SocialSkillDto(
    string Value
);

public record SocialAccountDto(
    SocialMediaTypes Type,
    string Address
);