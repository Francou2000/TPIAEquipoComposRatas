using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootView : MonoBehaviour
{
    public Image loot;

    public void AppearLoot()
    {
        
    }

    public void SetLoot(RarirtyEnum rarity, Sprite sprite)
    {
        loot.sprite = sprite;
    }

    public Sprite Image
    {
        get
        {
            return loot.sprite;
        }
        set
        {
            loot.sprite = value;
        }
    }
}
