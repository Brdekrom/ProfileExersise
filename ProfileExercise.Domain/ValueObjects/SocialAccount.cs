using System.Text.RegularExpressions;
using ProfileExercise.Domain.Abstractions;

namespace ProfileExercise.Domain.ValueObjects;

public sealed class SocialAccount : ValueObject
{
    private static readonly Regex TwitterHandleRegex = new(@"^@[A-Za-z0-9_]{1,15}$", RegexOptions.Compiled);
    private static readonly Regex SnapchatHandleRegex = new(@"^[A-Za-z0-9_.-]{3,15}$", RegexOptions.Compiled);

    public SocialAccount(SocialMediaTypes type, string address)
    {
        SocialMediaType = type;

        switch (type)
        {
            case SocialMediaTypes.None:
                if (!string.IsNullOrWhiteSpace(address))
                    throw new ArgumentException("No address expected when SocialMediaType is None.", nameof(address));
                Address = string.Empty;
                break;

            case SocialMediaTypes.Twitter:
                if (string.IsNullOrWhiteSpace(address) || !TwitterHandleRegex.IsMatch(address))
                    throw new ArgumentException(
                        "Twitter handles must match @username (1–15 letters, digits or underscores).",
                        nameof(address));
                Address = address;
                break;

            case SocialMediaTypes.Facebook:
                ValidateUrl(address, "facebook.com/",
                    "Facebook URLs must look like https://www.facebook.com/{username}/.");
                Address = address;
                break;

            case SocialMediaTypes.LinkedIn:
                ValidateUrl(address, "linkedin.com/in/",
                    "LinkedIn URLs must look like https://www.linkedin.com/in/{username}/.");
                Address = address;
                break;

            case SocialMediaTypes.Instagram:
                ValidateUrl(address, "instagram.com/",
                    "Instagram URLs must look like https://www.instagram.com/{username}/.");
                Address = address;
                break;

            case SocialMediaTypes.GitHub:
                ValidateUrl(address, "github.com/",
                    "GitHub URLs must look like https://github.com/{username}/.");
                Address = address;
                break;

            case SocialMediaTypes.YouTube:
                if (!Uri.TryCreate(address, UriKind.Absolute, out var youtube) ||
                    (!youtube.Host.Contains("youtube.com", StringComparison.OrdinalIgnoreCase) &&
                     !youtube.Host.Contains("youtu.be", StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ArgumentException(
                        "YouTube URLs must be either https://www.youtube.com/... or https://youtu.be/...",
                        nameof(address));
                }

                Address = address;
                break;

            case SocialMediaTypes.TikTok:
                ValidateUrl(address, "tiktok.com/",
                    "TikTok URLs must look like https://www.tiktok.com/@{username}/.");
                Address = address;
                break;

            case SocialMediaTypes.Snapchat:
                if (string.IsNullOrWhiteSpace(address) || !SnapchatHandleRegex.IsMatch(address))
                    throw new ArgumentException(
                        "Snapchat usernames must be 3–15 characters: letters, digits, '.', '_' or '-'.",
                        nameof(address));
                Address = address;
                break;

            case SocialMediaTypes.Pinterest:
                ValidateUrl(address, "pinterest.com/",
                    "Pinterest URLs must look like https://www.pinterest.com/{username}/.");
                Address = address;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported social media type: {type}");
        }
    }

    public SocialMediaTypes SocialMediaType { get; }
    public string Address { get; }

    private static void ValidateUrl(string address, string requiredSegment, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(address) ||
            !Uri.TryCreate(address, UriKind.Absolute, out var uri) ||
            !uri.AbsoluteUri.Contains(requiredSegment, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(errorMessage, nameof(address));
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SocialMediaType;
        yield return Address;
    }

    public override string ToString() => $"{SocialMediaType}: {Address}";
}