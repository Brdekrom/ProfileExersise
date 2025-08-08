using ProfileExercise.Domain.Abstractions;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Domain.Entities;

public sealed class Profile : Entity<Guid>
{
    private readonly List<SocialAccount> _socialAccounts;

    private readonly List<SocialSkill> _socialSkills;

    public Profile(
        Name firstName,
        Name lastName,
        IEnumerable<SocialSkill>? initialSkills = null,
        IEnumerable<SocialAccount>? initialAccounts = null
    )
    {
        FirstName = firstName;
        LastName = lastName;

        _socialSkills = (initialSkills ?? [])
            .Distinct()
            .ToList();

        _socialAccounts = new List<SocialAccount>();

        if (initialAccounts == null) return;
        foreach (var account in initialAccounts)
        {
            if (_socialAccounts.Any(a => a.SocialMediaType == account.SocialMediaType))
                continue;
            _socialAccounts.Add(account);
        }
    }


    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public IReadOnlyCollection<SocialSkill> SocialSkills => _socialSkills.AsReadOnly();
    public IReadOnlyCollection<SocialAccount> SocialAccounts => _socialAccounts.AsReadOnly();
}