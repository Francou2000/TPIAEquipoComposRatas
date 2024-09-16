﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour
{
    public Dictionary<RarirtyEnum, Sprite[]> cards = new Dictionary<RarirtyEnum, Sprite[]>();

    private void Awake()
    {
        var sr = Resources.LoadAll<Sprite>("Cards/SR");
        var r = Resources.LoadAll<Sprite>("Cards/R");
        var c = Resources.LoadAll<Sprite>("Cards/C");

        cards.Add(RarirtyEnum.SR, sr);
        cards.Add(RarirtyEnum.R, r);
        cards.Add(RarirtyEnum.C, c);
    }
}
