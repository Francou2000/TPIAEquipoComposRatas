using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool _hasItem = false;
    private GameObject _item;

    public void PickupItem(GameObject item)
    {
        _hasItem = true;
        _item = item;
    }

    public GameObject DropItem()
    {
        if (_hasItem && _item != null)
        {
            _hasItem = false;
            GameObject itemTo = _item;
            GameObject itemToDrop = _item;
            _item = null;
            return itemToDrop;
        }

        return null;
    }
}
