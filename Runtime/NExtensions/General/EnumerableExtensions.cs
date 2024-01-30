using System;

namespace NuiN.NExtensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this T[] arr, Action<T> action) => Array.ForEach(arr, action);
    }
}