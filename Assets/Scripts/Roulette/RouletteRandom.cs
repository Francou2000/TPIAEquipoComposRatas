using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteRandom
{
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
}
