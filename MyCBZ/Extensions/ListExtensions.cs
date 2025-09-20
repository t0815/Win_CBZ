using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Win_CBZ.Extensions
{
    public static class ListExtensions
    {
        public delegate void EachCallbackList<T>(T item, int? index);
        /// <summary>
        /// just like the forEach in JavaScript
        /// </summary>
        public static void Each<T>(this List<T> source, EachCallbackList<T> callback)
        {
            for (int i = 0; i < source.Count; i++)
            {
                callback(source[i], i);
            }
        }
    }
}
