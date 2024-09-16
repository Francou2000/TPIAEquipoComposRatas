using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public LootRandom lootRandom;             
    public PlayerInventory playerInventory;   
    public LootView lootView;                 

    public void Interact()
    {
        if (!playerInventory._hasItem)
        {
            DropItem();
        }
    }

    private void DropItem()
    {
        lootRandom.GetRandomLoot();

        Sprite droppedItem = lootView.Image;

        if (droppedItem != null)
        {
            playerInventory.PickupItem(droppedItem);
        }
    }
}
