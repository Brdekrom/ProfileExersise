using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Tests.ValueObjects;

public sealed class SocialSkillTests
{
    [Theory]
    [InlineData("Communicatie")]
    [InlineData("Teamwork")]
    [InlineData(" Probleemoplossend vermogen ")]
    [InlineData("Creativiteit")] // accenten zouden ook werken
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
        Assert.Equal("value", ex.ParamName); // komt overeen met ctor parameter
        Assert.Contains("must be provided", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Ctor_Accepts_Exactly_Max_Length()
    {
        var s = new string('x', 50);
        var skill = new SocialSkill(s);

        Assert.Equal(50, skill.Value.Length);
        Assert.Equal(s, skill.Value);
    }

    [Fact]
    public void Equality_Is_Based_On_Value()
    {
        var a = new SocialSkill("Communicatie");
        var b = new SocialSkill("Communicatie");
        var c = new SocialSkill("Leiderschap");

        Assert.True(a == b);
        Assert.False(a != b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());

        Assert.False(a == c);
        Assert.True(a != c);
        Assert.NotEqual(a.GetHashCode(), c.GetHashCode());
    }

    [Fact]
    public void ToString_Returns_Value()
    {
        var skill = new SocialSkill("Teamwork");
        Assert.Equal("Teamwork", skill.ToString());
    }
}