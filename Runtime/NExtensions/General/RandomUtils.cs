using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NuiN.NExtensions
{
    public static class RandomUtils
    {
        public static bool Bool()
        {
            int rand = Random.Range(0, 2);
            return rand != 0;
        }

        public static int Sign()
        {
            bool flag = Bool();
            return flag == false ? -1 : 1;
        }

        public static float Multiplier(float randomness)
        {
            return Random.Range(1 - randomness, 1 + randomness);
        }
        
        public static T Enum<T>() where T : Enum
        {
            Array enumArray = System.Enum.GetValues(typeof(T));
            T value = (T)enumArray.GetValue(Random.Range(0, enumArray.Length));
            return value;
        }

        public static Quaternion Rotation2D()
        {
            return Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        }

        public static bool BelowPercent(float percent)
        {
            return Random.Range(0f, 100f) <= percent;
        }
        public static bool AbovePercent(float percent)
        {
            return Random.Range(0f, 100f) >= percent;
        }
    }

    public static class RandomExtensions
    {
        public static T RandomItem<T>(this List<T> list)
        {
            return list[list.RandomIndex()];
        }
        public static T RandomItem<T>(this T[] array)
        {
            return array[array.RandomIndex()];
        }

        public static int RandomIndex<T>(this IEnumerable<T> collection)
        {
            int count = collection.Count();
            if (count == 0) throw new IndexOutOfRangeException("List was empty when retrieving a random item");
            return Random.Range(0, count);
        }
    }
}