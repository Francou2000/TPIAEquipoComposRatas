using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBase : MonoBehaviour

{
    public Dictionary<RarirtyEnum, GameObject[]> items = new Dictionary<RarirtyEnum, GameObject[]>();

    private void Awake()
    {
        var sr = Resources.LoadAll<GameObject>("Items/SR");
        var r = Resources.LoadAll<GameObject>("Items/R");
        var c = Resources.LoadAll<GameObject>("Items/C");

        items.Add(RarirtyEnum.SR, sr);
        items.Add(RarirtyEnum.R, r);
        items.Add(RarirtyEnum.C, c);
    }
}