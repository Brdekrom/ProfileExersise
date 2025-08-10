using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Tests.ValueObjects;

public sealed class SocialSkillTests
{
    [Theory]
    [InlineData("C#")]
    [InlineData("Azure")]
    [InlineData(" Problem Solving ")]
    public void Ctor_Accepts_Valid_Values_And_Trims(string input)
    {
        var skill = new SocialSkill(input);

        Assert.Equal(input.Trim(), skill.Value);
        Assert.Equal(input.Trim(), skill.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public void Ctor_Rejects_Empty_Or_Whitespace(string input)
    {
        var ex = Assert.Throws<ArgumentException>(() => new SocialSkill(input));
        Assert.Equal("value", ex.ParamName);
        Assert.Contains("must be provided", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Ctor_Accepts_Exactly_Max_Length()
    {
        // MaxLength = 50
        var s = new string('x', 50);

        var skill = new SocialSkill(s);

        Assert.Equal(50, skill.Value.Length);
        Assert.Equal(s, skill.Value);
    }

    [Fact]
    public void Ctor_Rejects_Above_Max_Length()
    {
        var over = new string('x', 51);

        var ex = Assert.Throws<ArgumentException>(() => new SocialSkill(over));
        Assert.Equal("value", ex.ParamName);
        Assert.Contains("at most 50 characters", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Equality_Is_Based_On_Value()
    {
        var a = new SocialSkill("Friendly");
        var b = new SocialSkill("Friendly");
        var c = new SocialSkill("Test");

        Assert.Equal(a, b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());

        Assert.NotEqual(a, c);
        Assert.NotEqual(a.GetHashCode(), c.GetHashCode());
    }

    [Fact]
    public void ToString_Returns_Value()
    {
        var skill = new SocialSkill("Clean Architecture");
        Assert.Equal("Clean Architecture", skill.ToString());
    }
}