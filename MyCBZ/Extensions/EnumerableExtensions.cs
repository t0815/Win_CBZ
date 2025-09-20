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
        public delegate bool EachUntilCallbackEnumerable<T>(T item);

        public delegate void EachCallbackEnumerable<T>(T item);

        public delegate T MapCallbackEnumerable<T>(T item);


        /// <summary>
        /// just like the forEach in JavaScript, but with a callback that
        /// returns a boolean to indicate whether to continue iterating or not.
        /// (True to continue, False to break)
        /// </summary>

        public static void EachUntil<T>(this IEnumerable<T> source, EachUntilCallbackEnumerable<T> callback)
        {
            foreach (T item in source)
            {
                if (!callback(item))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// just like the forEach in JavaScript
        /// </summary>
        public static void Each<T>(this IEnumerable<T> source, EachCallbackEnumerable<T> callback)
        {
            foreach (T item in source)
            {
                callback(item);
            }
        }


        /// <summary>
        /// 
        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, MapCallbackEnumerable<T> callback)
        {
            foreach (T item in source)
            {
                yield return callback(item);
            }
        }
    }
}
