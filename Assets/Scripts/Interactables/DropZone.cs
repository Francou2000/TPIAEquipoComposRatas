using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public PlayerInventory _inventory;
    public int _playerPoints = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _inventory._hasItem)
        {
            GameObject droppedItem = _inventory.DropItem();

            LootableItem lootableItem = droppedItem.GetComponent<LootableItem>();
            if (lootableItem != null )
            {
                _playerPoints += lootableItem._itemValue;

                Destroy(droppedItem);
            }
        }
    }
}
