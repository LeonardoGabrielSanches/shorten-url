namespace UrlShortener.Domain.Utils;

public static class CodeGenerator
{
    private static readonly Random Random = new();
    private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomCode(int length)
    {
        return new string(Enumerable.Repeat(CHARS, length)
            .Select(s => s[Random.Next(s.Length)])
            .ToArray());
    }
}