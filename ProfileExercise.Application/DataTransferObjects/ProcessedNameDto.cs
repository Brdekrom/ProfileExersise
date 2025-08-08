namespace ProfileExercise.Application.DataTransferObjects;

public record ProcessedNameDto(
    int VowelCount,
    int ConsonantCount,
    string ReversedFirstName,
    string ReversedLastName
);