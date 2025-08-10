using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Tests.ValueObjects;

public sealed class NameTests
{
    [Theory]
    [InlineData("john", "John")]
    [InlineData("  john   doe  ", "John Doe")]
    [InlineData("éloïse", "Éloïse")]
    [InlineData("jean-luc", "Jean-Luc")]
    [InlineData("maría josé", "María José")]
    public void Ctor_Normalizes_To_TitleCase_And_Collapses_Spaces(string input, string expected)
    {
        var name = new Name(input);

        Assert.Equal(expected, name.Value);
        Assert.Equal(expected, name.ToString());
    }

    [Theory]
    [InlineData("O'neill")]
    [InlineData("D'Artagnan")]
    [InlineData("Anne-Marie")]
    [InlineData("Åsa")]
    [InlineData("François Hollande")]
    public void Allows_Letters_Spaces_Hyphen_Apostrophe_Accents(string input)
    {
        var act = () => new Name(input);
        var ex = Record.Exception(act);
        Assert.Null(ex);
    }

    [Theory]
    [InlineData("John3")]
    [InlineData("John_Doe")]
    [InlineData("John!")]
    [InlineData("John@Doe")]
    [InlineData("John, Doe")]
    public void Rejects_Invalid_Characters(string input)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Name(input));
        Assert.Equal("value", ex.ParamName);
        Assert.Contains("letters, spaces, hyphens, or apostrophes", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    public void Rejects_Empty_Or_Whitespace(string input)
    {
        var ex = Assert.Throws<ArgumentException>(() => new Name(input));
        Assert.Equal("value", ex.ParamName);
        Assert.Contains("must be provided", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Rejects_Too_Long()
    {
        var over = new string('a', 51);
        var ex = Assert.Throws<ArgumentException>(() => new Name(over));
        Assert.Equal("value", ex.ParamName);
        Assert.Contains("at most 50 characters", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Accepts_At_Max_Length()
    {
        var exactlyMax = new string('a', 50);
        var name = new Name(exactlyMax);

        Assert.Equal("A" + new string('a', 49), name.Value);
    }

    [Fact]
    public void Collapses_Internal_Multiple_Spaces_To_Single()
    {
        var name = new Name("  john    ronald   reuel   tolkien  ");
        Assert.Equal("John Ronald Reuel Tolkien", name.Value);
    }

    [Fact]
    public void Equality_Is_Based_On_Normalized_Value()
    {
        var a = new Name("  jOhn   doE ");
        var b = new Name("john doe");
        var c = new Name("John  Doe");

        Assert.Equal(a, b);
        Assert.Equal(b, c);
        Assert.Equal(a, c);

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
        Assert.Equal(b.GetHashCode(), c.GetHashCode());
    }

    [Fact]
    public void Inequality_When_Different_After_Normalization()
    {
        var a = new Name("John Doe");
        var b = new Name("John Poe");

        Assert.NotEqual(a, b);
        Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void ToString_Returns_Value()
    {
        var n = new Name("éloïse  martin");
        Assert.Equal(n.Value, n.ToString());
    }
}