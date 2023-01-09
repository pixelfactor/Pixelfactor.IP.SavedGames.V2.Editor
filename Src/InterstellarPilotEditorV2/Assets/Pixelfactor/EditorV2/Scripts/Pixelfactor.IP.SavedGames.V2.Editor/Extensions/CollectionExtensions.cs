using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pixelfactor.IP.SavedGames.V2.Editor
{
    public static class CollectionExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            if (list != null)
            {
                var count = list.Count();
                if (count > 0)
                {
                    return list.ElementAt(Random.Range(0, count));
                }
            }

            return default(T);
        }

        public static T GetRandom<T>(this IEnumerable<T> list, System.Random random)
        {
            if (list != null)
            {
                var count = list.Count();
                if (count > 0)
                {
                    return list.ElementAt(random.Next(0, count));
                }
            }

            return default(T);
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            if (list != null && list.Count > 0)
            {
                return list[Random.Range(0, list.Count)];
            }

            return default(T);
        }

        public static T GetRandom<T>(this IList<T> list, int max)
        {
            if (list != null && list.Count > 0)
            {
                return list[Random.Range(0, max)];
            }

            return default(T);
        }

        public static int IndexOf<T>(this IList<T> list, T item)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (Equals(list[i], item))
                {
                    return i;
                }
            }

            return -1;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var copy = list.ToList();
            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, copy.Count);
                list[i] = copy[index];
                copy.RemoveAt(index);
            }
        }

        public static void Shuffle<T>(this IList<T> list, System.Random random)
        {
            var copy = list.ToList();
            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var index = random.Next(0, copy.Count);
                list[i] = copy[index];
                copy.RemoveAt(index);
            }
        }
    }
}
