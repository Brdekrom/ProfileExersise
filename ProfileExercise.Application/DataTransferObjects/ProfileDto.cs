using ProfileExercise.Domain;
using ProfileExercise.Domain.Entities;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Application.DataTransferObjects;

public record ProfileDto(
    Guid Id,
    string FirstName,
    string LastName,
    IReadOnlyList<SocialSkillDto> SocialSkills,
    IReadOnlyList<SocialAccountDto> SocialAccounts
)
{
    public static implicit operator Profile(ProfileDto dto)
    {
        var firstNameVo = new Name(dto.FirstName);
        var lastNameVo = new Name(dto.LastName);

        var profile = new Profile(firstNameVo, lastNameVo,
            dto.SocialSkills.Select(s => new SocialSkill(s.Value)),
            dto.SocialAccounts.Select(a => new SocialAccount(a.Type, a.Address)))
        {
            Id = dto.Id
        };

        return profile;
    }

    public static implicit operator ProfileDto(Profile profile)
    {
        return new ProfileDto(
            profile.Id,
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