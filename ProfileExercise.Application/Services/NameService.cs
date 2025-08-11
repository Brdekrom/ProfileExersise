using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Application.Services;

internal class NameService : INameService
{
    private static readonly HashSet<char> Vowels = new() { 'a', 'e', 'i', 'o', 'u' };

    public ProcessedNameDto Process(Name firstName, Name lastName)
    {
        var reversedFirstName = Reverse(firstName.Value);
        var reversedLastName = Reverse(lastName.Value);

        var originalFull = $"{firstName.Value} {lastName.Value}";
        var countResult = CountLetters(originalFull);

        return new ProcessedNameDto(
            countResult.VowelCount,
            countResult.ConsonantCount,
            reversedFirstName,
            reversedLastName
        );
    }

    private string Reverse(string input) => new(input.ToCharArray().Reverse().ToArray());

    private CountResult CountLetters(string input)
    {
        var vowels = 0;
        var consonants = 0;

        foreach (var c in input)
        {
            var lower = char.ToLowerInvariant(c);
            if (!char.IsLetter(lower))
                continue;

            if (Vowels.Contains(lower))
                vowels++;
            else
                consonants++;
        }

        return new CountResult(vowels, consonants);
    }

    private record CountResult(int VowelCount, int ConsonantCount);
}