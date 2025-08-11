using Microsoft.EntityFrameworkCore;
using Moq;
using ProfileExercise.Application.Abstractions;
using ProfileExercise.Application.Commands;
using ProfileExercise.Application.DataTransferObjects;
using ProfileExercise.Application.Tests.Helpers;
using ProfileExercise.Domain;
using ProfileExercise.Domain.Entities;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Application.Tests.Command;

public sealed class CreateProfileCommandHandlerTests
{
    [Fact]
    public async Task Handle_AddsProfile_SavesChanges_AndProcessesName()
    {
        // Arrange
        var firstName = "Alice";
        var lastName = "Smith";

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase($"profiles-{Guid.NewGuid()}")
            .Options;

        await using var db = new TestDbContext(options);
        var repo = new FakeRepository(db);

        var nameService = new Mock<INameService>();
        var processed = new ProcessedNameDto(4, 6, "ecilA", "htimS");

        nameService
            .Setup(s => s.Process(It.IsAny<Name>(), It.IsAny<Name>()))
            .Returns(processed);

        var handler = new CreateProfileCommandHandler(repo, nameService.Object);

        var socialSkills = new List<SocialSkillDto> { new("Teamwork"), new("Communication") };
        var socialAccounts = new List<SocialAccountDto>
        {
            new(SocialMediaTypes.Twitter, "@alice"),
            new(SocialMediaTypes.LinkedIn, "https://linkedin.com/in/alice")
        };

        var dto = new ProfileDto(
            Id: null,
            FirstName: firstName,
            LastName: lastName,
            SocialSkills: socialSkills,
            SocialAccounts: socialAccounts);

        var cmd = new CreateProfileCommand(dto);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        var stored = await db.Set<Profile>().SingleAsync();
        Assert.Equal(firstName, stored.FirstName.Value);
        Assert.Equal(lastName, stored.LastName.Value);
        Assert.NotEqual(Guid.Empty, stored.Id);

        nameService.Verify(s => s.Process(
            It.Is<Name>(n => n.Value == firstName),
            It.Is<Name>(n => n.Value == lastName)), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(processed.VowelCount, result.ProcessedName.VowelCount);
        Assert.Equal(processed.ConsonantCount, result.ProcessedName.ConsonantCount);
        Assert.Equal(processed.ReversedFirstName, result.ProcessedName.ReversedFirstName);
        Assert.Equal(processed.ReversedLastName, result.ProcessedName.ReversedLastName);
    }

    [Fact]
    public async Task Handle_UsesProvidedGuidId_WhenDtoHasValidId()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";

        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase($"profiles-{Guid.NewGuid()}")
            .Options;

        await using var db = new TestDbContext(options);
        var repo = new FakeRepository(db);

        var nameService = new Mock<INameService>();
        nameService
            .Setup(s => s.Process(It.IsAny<Name>(), It.IsAny<Name>()))
            .Returns(new ProcessedNameDto(3, 4, "nhoJ", "eoD"));

        var handler = new CreateProfileCommandHandler(repo, nameService.Object);

        var fixedId = Guid.NewGuid().ToString();
        var dto = new ProfileDto(
            Id: fixedId,
            FirstName: firstName,
            LastName: lastName,
            SocialSkills: Array.Empty<SocialSkillDto>(),
            SocialAccounts: Array.Empty<SocialAccountDto>());

        var cmd = new CreateProfileCommand(dto);

        // Act
        var result = await handler.Handle(cmd, CancellationToken.None);

        // Assert
        var stored = await db.Set<Profile>().SingleAsync();
        Assert.Equal(Guid.Parse(fixedId), stored.Id);

        nameService.Verify(s => s.Process(
            It.Is<Name>(n => n.Value == firstName),
            It.Is<Name>(n => n.Value == lastName)), Times.Once);

        Assert.NotNull(result);
        Assert.Equal("nhoJ", result.ProcessedName.ReversedFirstName);
        Assert.Equal("eoD", result.ProcessedName.ReversedLastName);
    }
}