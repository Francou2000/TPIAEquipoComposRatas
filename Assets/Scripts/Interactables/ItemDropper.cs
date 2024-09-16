using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public LootRandom lootRandom;             
    public PlayerInventory playerInventory;  

    public void Interact()
    {
        if (!playerInventory._hasItem)
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        lootRandom.GetRandomItem();

        playerInventory.PickupItem();
    }
}
