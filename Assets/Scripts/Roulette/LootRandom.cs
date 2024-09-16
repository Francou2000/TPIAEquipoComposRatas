using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LootRandom : MonoBehaviour
{
    public DataBase dataBase;
    public List<RarityInfo> infos;
    public Transform spawnPoint;  

    Dictionary<RarirtyEnum, float> _items;

    private void Awake()
    {
        _items = new Dictionary<RarirtyEnum, float>();
        for (int i = 0; i < infos.Count; i++)
        {
            _items[infos[i].type] = infos[i].weight;
        }
    }

    public GameObject GetRandomItem()
    {
        RarirtyEnum rarity = RouletteRandom.Roulette(_items);
        return SpawnLoot(rarity);
    }

    private GameObject SpawnLoot(RarirtyEnum rarity)
    {
        if (!dataBase) return null;
        if (!dataBase.items.ContainsKey(rarity)) return null;

        GameObject[] items = dataBase.items[rarity];

        int randomIndex = UnityEngine.Random.Range(0, items.Length);
        GameObject selectedItem = items[randomIndex];

        GameObject spawnedItem = Instantiate(selectedItem, spawnPoint.position, Quaternion.identity);

        LootableItem lootable = spawnedItem.GetComponent<LootableItem>();
        if (lootable != null)
        {
            int itemValue = infos.Find(info => info.type == rarity).value;
            lootable.SetItemValue(itemValue);
        }

        return spawnedItem;
    }
}