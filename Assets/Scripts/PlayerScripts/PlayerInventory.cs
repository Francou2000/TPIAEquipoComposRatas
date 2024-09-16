using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool _hasItem = false;
    public Sprite _currentItemSprite;

    public void PickupItem(Sprite itemSprite)
    {
        _hasItem = true;
        _currentItemSprite = itemSprite;
    }

    public void DropItem()
    {
        _hasItem = false;
        _currentItemSprite = null;
    }
}
