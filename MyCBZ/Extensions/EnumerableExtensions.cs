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
        public delegate bool EachCallback<T>(T item);

        public delegate T MapCallback<T>(T item);


        /// <summary>
        /// just like the forEach in JavaScript, but with a callback that
        /// returns a boolean to indicate whether to continue iterating or not.
        /// </summary>

        public static void Each<T>(this IEnumerable<T> source, EachCallback<T> callback)
        {
           foreach (T item in source)
           {
                if (callback(item))
                {
                    break;
                }
           }
        }


        /// <summary>
        /// 
        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, MapCallback<T> callback)
        {
            foreach (T item in source)
            {
                yield return callback(item);
            }
        }
    }
}
