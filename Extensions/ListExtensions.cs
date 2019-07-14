using System;
using System.Collections.Generic;

namespace TLoZ.Extensions
{
    public static class ListExtensions
    {
        public static T Previous<T>(this IList<T> list, T current) => Previous<T>(list, current, true);

        public static T Previous<T>(this IList<T> list, T current, bool loop)
        {
            int index = list.IndexOf(current);

            if (index == 0)
            {
                if (loop)
                    return list[list.Count - 1];

                throw new IndexOutOfRangeException("There are no previous elements in the sequence and looping is disabled.");
            }
            else
                return list[index - 1];
        }

        public static T Next<T>(this IList<T> list, T current) => Previous<T>(list, current, true);

        public static T Next<T>(this IList<T> list, T current, bool loop)
        {
            int index = list.IndexOf(current);

            if (index == list.Count - 1)
            {
                if (loop)
                    return list[0];

                throw new IndexOutOfRangeException("There are no next elements in the sequence and looping is disabled.");
            }
            else
                return list[index + 1];
        }
    }
}