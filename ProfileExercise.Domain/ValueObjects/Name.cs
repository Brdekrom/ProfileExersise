using System.Text.RegularExpressions;
using ProfileExercise.Domain.Abstractions;

namespace ProfileExercise.Domain.ValueObjects;

public sealed class Name : ValueObject
{
    private const int MaxLength = 50;

    private static readonly Regex ValidNameRegex =
        new(@"^[A-Za-zÀ-ÖØ-öø-ÿ\-']{1,50}$", RegexOptions.Compiled);

    public Name(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name must be provided.", nameof(value));

        var trimmed = value.Trim();
        if (trimmed.Length > MaxLength)
            throw new ArgumentException(
                $"Name must be at most {MaxLength} characters.",
                nameof(value));

        if (!ValidNameRegex.IsMatch(trimmed))
            throw new ArgumentException(
                "Name may only contain letters, hyphens, or apostrophes.",
                nameof(value));

        Value = char.ToUpperInvariant(trimmed[0])
                + trimmed.Substring(1).ToLowerInvariant();
    }

    public string Value { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}