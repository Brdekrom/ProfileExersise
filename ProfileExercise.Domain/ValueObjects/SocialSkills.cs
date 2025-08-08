using ProfileExercise.Domain.Abstractions;

namespace ProfileExercise.Domain.ValueObjects;

public sealed class SocialSkill : ValueObject
{
    private const int MaxLength = 50;

    public SocialSkill(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Skill name must be provided.", nameof(value));

        value = value.Trim();

        if (value.Length > MaxLength)
            throw new ArgumentException($"Skill name must be at most {MaxLength} characters.", nameof(value));

        Value = value;
    }

    public string Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}