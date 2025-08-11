using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Application.Abstractions;

public interface INameService
{
    ProcessedNameDto Process(Name firstName, Name lastName);
}