using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Application.Abstractions;

public interface IAnalyzer
{
    ProcessedNameDto Process(Name firstName, Name lastName);
}