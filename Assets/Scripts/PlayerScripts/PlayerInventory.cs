using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool _hasItem = false;

    public void PickupItem()
    {
        _hasItem = true;
    }

    public void DropItem()
    {
        _hasItem = false;
    }
}
