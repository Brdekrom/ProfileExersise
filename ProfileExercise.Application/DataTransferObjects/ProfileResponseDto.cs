namespace ProfileExercise.Application.DataTransferObjects;

public record ProfileResponseDto(
    ProfileDto Profile,
    ProcessedNameDto ProcessedName
);