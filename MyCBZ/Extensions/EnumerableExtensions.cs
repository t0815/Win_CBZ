using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Win_CBZ.Extensions.EnumerableExtensions;

namespace Win_CBZ.Extensions
{
    public static class EnumerableExtensions
    {
        public delegate void EachCallback<T>(T item);

        public delegate T MapCallback<T>(T item);

        public static void Each<T>(this IEnumerable<T> source, EachCallback<T> callback)
        {
           foreach (T item in source)
           {
                callback(item);
           }
        }

        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, MapCallback<T> callback)
        {
            foreach (T item in source)
            {
                yield return callback(item);
            }
        }
    }
}
