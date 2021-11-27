using System.Collections.Generic;
using System.Linq;

namespace DependencyAnalyzer.Extensions
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> SkipNull<T>(this IEnumerable<T> source)
    {
      return source.Where(item => item != null);
    }

    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
      return Enumerable.Repeat(item, 1);
    }
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable)
    {
      return enumerable ?? Enumerable.Empty<T>();
    }

    public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item)
    {
      return enumerable.Concat(item.ToEnumerable());
    }
  }
}