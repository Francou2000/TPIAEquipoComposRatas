using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public LootRandom _lootRandom;             
    public PlayerInventory _inventory;  

    public void Interact()
    {
        if (!_inventory._hasItem)
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        GameObject droppedItem = _lootRandom.GetRandomItem();

        if (droppedItem != null)
        {
            _inventory.PickupItem(droppedItem);
        }
    }
}
