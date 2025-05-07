namespace Application.Extensions;

public static class EnumerableExtensions
{
    public static void ForEach<TElement>(this IEnumerable<TElement> enumerable, Action<TElement> action)
    {
        foreach (var element in enumerable)
        {
            action(element);
        }
    }

    public static bool IsEmpty<TElement>(this IEnumerable<TElement> enumerable) => !enumerable.Any();
}
