using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandoms
{
    public static float Range(float min, float max)
    {
        return min + UnityEngine.Random.value * (max - min);
    }

    public static T Roulette<T>(Dictionary<T, float> items)
    {
        float total = 0;
        foreach (var item in items)
        {
            total += item.Value;
        }
        float random = UnityEngine.Random.Range(0, total);
        foreach (var item in items)
        {
            random -= item.Value;
            if (random <= 0)
            {
                return item.Key;
            }
        }

        return default;
    }

    public static void Shuffle<T>(List<T> items, Action<T, T> onSwap = null)
    {
        for (int i = 0; i < items.Count; i++)
        {

            int r = UnityEngine.Random.Range(i, items.Count);
            if (onSwap != null)
            {
                onSwap(items[i], items[r]);
            }
            T aux = items[r];
            items[r] = items[i];
            items[i] = aux;
        }
    }
    
}
