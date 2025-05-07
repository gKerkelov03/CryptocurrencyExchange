
namespace Application.Extensions;

public static class StringExtensions
{
    public static string WithFirstLetterToLower(this string text)
    {
        var lowercaseFirstLetter = text[0].ToString().ToLower();
        var otherPart = string.Join("", text.Skip(1));

        return lowercaseFirstLetter + otherPart;
    }

    public static string Remove(this string text, IEnumerable<char> charsToRemove)
    {
        charsToRemove.ForEach(@char => text = text.Replace(@char.ToString(), string.Empty));

        return text;
    }
}

