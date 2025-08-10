using ProfileExercise.Domain;
using ProfileExercise.Domain.ValueObjects;

namespace ProfileExercise.Tests.ValueObjects;

public sealed class SocialAccountTests
{
    // -------- None --------
    [Fact]
    public void None_Allows_Empty_Address()
    {
        var acc = new SocialAccount(SocialMediaTypes.None, "");
        Assert.Equal(SocialMediaTypes.None, acc.SocialMediaType);
        Assert.Equal("", acc.Address);
    }

    // -------- Twitter --------
    [Theory]
    [InlineData("@a")]
    [InlineData("@user_123")]
    public void Twitter_Allows_1_To_15_Alnum_Underscore(string handle)
    {
        Assert.True(handle.Length <= 16, "Test data invariant: handle (incl @) must be <= 16.");
        var acc = new SocialAccount(SocialMediaTypes.Twitter, handle);
        Assert.Equal(handle, acc.Address);
    }

    [Theory]
    [InlineData("")]
    [InlineData("user")]
    [InlineData("@")]
    [InlineData("@this_is_too_long_handle")]
    [InlineData("@has-dash")]
    [InlineData("@has.dot")]
    public void Twitter_Rejects_Invalid_Handles(string handle)
    {
        var ex = Assert.Throws<ArgumentException>(() => new SocialAccount(SocialMediaTypes.Twitter, handle));
        Assert.Equal("address", ex.ParamName);
        Assert.Contains("Twitter handles", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------- Snapchat --------
    [Theory]
    [InlineData("abc")]
    [InlineData("user.name")]
    [InlineData("user-name")]
    [InlineData("user_name")]
    [InlineData("Name123")]
    [InlineData("a23456789012345")] // 15
    public void Snapchat_Allows_3_To_15_Of_Specified_Charset(string username)
    {
        var acc = new SocialAccount(SocialMediaTypes.Snapchat, username);
        Assert.Equal(username, acc.Address);
    }

    [Theory]
    [InlineData("")]
    [InlineData("ab")] // <3
    [InlineData("a234567890123456")] // 16
    [InlineData("name!")] // illegal char
    [InlineData("white space")] // space not allowed
    public void Snapchat_Rejects_Invalid_Usernames(string username)
    {
        var ex = Assert.Throws<ArgumentException>(() => new SocialAccount(SocialMediaTypes.Snapchat, username));
        Assert.Equal("address", ex.ParamName);
        Assert.Contains("Snapchat usernames", ex.Message, StringComparison.OrdinalIgnoreCase);
    }


    [Theory]
    [InlineData(SocialMediaTypes.Facebook, "https://example.com/john")]
    [InlineData(SocialMediaTypes.LinkedIn, "https://linkedin.com/company/acme")] // needs /in/
    [InlineData(SocialMediaTypes.Instagram, "https://example.com/john")]
    [InlineData(SocialMediaTypes.GitHub, "https://example.com/john")]
    [InlineData(SocialMediaTypes.TikTok, "https://example.com/@john")]
    [InlineData(SocialMediaTypes.Pinterest, "https://example.com/john")]
    public void UrlBased_Types_Reject_Invalid_Segments(SocialMediaTypes type, string url)
    {
        var ex = Assert.Throws<ArgumentException>(() => new SocialAccount(type, url));
        Assert.Equal("address", ex.ParamName);
        Assert.Contains("must look like", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------- YouTube (special host check) --------
    [Theory]
    [InlineData("https://www.youtube.com/@john")]
    [InlineData("https://youtube.com/watch?v=abc")]
    [InlineData("https://youtu.be/abc")]
    public void YouTube_Accepts_YouTube_Or_Shortened_Hosts(string url)
    {
        var acc = new SocialAccount(SocialMediaTypes.YouTube, url);
        Assert.Equal(url, acc.Address);
    }

    [Theory]
    [InlineData("")]
    [InlineData("https://example.com/video")]
    [InlineData("youtube.com/watch?v=abc")] // Relative URL will have Host empty; Validate is Absolute for YT
    public void YouTube_Rejects_Invalid_Urls(string url)
    {
        var ex = Assert.Throws<ArgumentException>(() => new SocialAccount(SocialMediaTypes.YouTube, url));
        Assert.Equal("address", ex.ParamName);
        Assert.Contains("YouTube URLs must be either", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    // -------- Equality & ToString --------
    [Fact]
    public void Equality_Based_On_Type_And_Address()
    {
        var a = new SocialAccount(SocialMediaTypes.GitHub, "https://github.com/john");
        var b = new SocialAccount(SocialMediaTypes.GitHub, "https://github.com/john");
        var c = new SocialAccount(SocialMediaTypes.GitHub, "https://github.com/jane");

        Assert.Equal(a, b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
        Assert.NotEqual(a, c);
    }

    [Fact]
    public void ToString_Contains_Type_And_Address()
    {
        var acc = new SocialAccount(SocialMediaTypes.LinkedIn, "https://www.linkedin.com/in/john-doe/");
        var s = acc.ToString();
        Assert.Contains(nameof(SocialMediaTypes.LinkedIn), s, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("linkedin.com/in/john-doe", s, StringComparison.OrdinalIgnoreCase);
    }

    // -------- Unsupported enum --------
    [Fact]
    public void Unsupported_SocialMediaType_Throws()
    {
        var invalid = (SocialMediaTypes)999;
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new SocialAccount(invalid, "anything"));
        Assert.Equal("socialMediaType", ex.ParamName);
    }
}