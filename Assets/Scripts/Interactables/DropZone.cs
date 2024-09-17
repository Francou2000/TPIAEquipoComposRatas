using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
    public PlayerInventory _inventory;
    public int _playerPoints = 0;
    
    public ScenesManagement _scenesManagement;

    public AudioSource _audioSource;
    public AudioClip _moneySound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _inventory._hasItem)
        {
            GameObject droppedItem = _inventory.DropItem();

            _audioSource.PlayOneShot(_moneySound);

            LootableItem lootableItem = droppedItem.GetComponent<LootableItem>();
            if (lootableItem != null )
            {
                _playerPoints += lootableItem._itemValue;

                Destroy(droppedItem);

                if ( _scenesManagement != null && _playerPoints >= 100)
                {
                    _scenesManagement.LoadScene("Victory");
                }
            }
        }
    }
}
