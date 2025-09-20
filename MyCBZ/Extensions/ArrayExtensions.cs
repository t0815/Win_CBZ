using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Win_CBZ.Extensions.EnumerableExtensions;

namespace Win_CBZ.Extensions
{
    public static class ArrayExtensions
    {
        public delegate void EachCallbackArray<T>(T item);

        public static void Each<T>(this T[] source, EachCallbackArray<T> callback)
        {
            foreach (T item in source)
            {
                callback(item);
            }
        }
    }
}
