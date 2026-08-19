namespace Application.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string compareA, string compareB)
    {
        return !string.IsNullOrWhiteSpace(compareA) && compareA.Equals(compareB, StringComparison.OrdinalIgnoreCase);
    }
}
