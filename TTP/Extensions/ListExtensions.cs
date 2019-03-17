using System;
using System.Collections.Generic;

namespace TTP.Extensions
{
    public static class ListExtensions
    {
        private static Random rng = new Random();

        public static List<T> Shuffle<T>(this List<T> list)
        {
            List<T> listClone = new List<T>(list);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = listClone[k];
                listClone[k] = listClone[n];
                listClone[n] = value;
            }

            return listClone;
        }

        public static void Swap<T>(this List<T> list)
        {
            var a = rng.Next(0, list.Count);
            var b = rng.Next(0, list.Count);
            T temp = list[a];
            list[a] = list[b];
            list[b] = temp;
        }

        public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}