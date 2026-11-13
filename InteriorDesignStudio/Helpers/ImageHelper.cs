using System.Text.RegularExpressions;

namespace InteriorDesignStudio.Helpers;

/// <summary>Builds responsive image URLs for remote placeholders and local files.</summary>
public static partial class ImageHelper
{
    public const string Fallback = "/images/placeholder.svg";

    /// <summary>
    /// Returns a width-optimised URL. Unsplash URLs get their "w" parameter rewritten so
    /// thumbnails download smaller files; local paths are returned unchanged.
    /// </summary>
    public static string Sized(string? path, int width)
    {
        if (string.IsNullOrWhiteSpace(path)) return Fallback;
        if (!path.Contains("images.unsplash.com", StringComparison.OrdinalIgnoreCase)) return path;

        return WidthParam().IsMatch(path)
            ? WidthParam().Replace(path, $"w={width}")
            : path + (path.Contains('?') ? "&" : "?") + $"w={width}";
    }

    /// <summary>Builds a srcset string for responsive images.</summary>
    public static string? SrcSet(string? path, params int[] widths)
    {
        if (string.IsNullOrWhiteSpace(path) ||
            !path.Contains("images.unsplash.com", StringComparison.OrdinalIgnoreCase))
            return null;
        return string.Join(", ", widths.Select(w => $"{Sized(path, w)} {w}w"));
    }

    [GeneratedRegex(@"(?<=[?&])w=\d+")]
    private static partial Regex WidthParam();
}
