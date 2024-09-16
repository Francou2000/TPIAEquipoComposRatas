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

    public void GetRandomItem()
    {
        RarirtyEnum rarity = RouletteRandom.Roulette(_items);
        SpawnLoot(rarity);
    }

    void SpawnLoot(RarirtyEnum rarity)
    {
        if (!dataBase) return;
        if (!dataBase.items.ContainsKey(rarity)) return;
        Debug.Log(rarity);

        GameObject[] items = dataBase.items[rarity];

        int randomIndex = UnityEngine.Random.Range(0, items.Length);
        GameObject selectedItem = items[randomIndex];

        Instantiate(selectedItem, spawnPoint.position, Quaternion.identity);
    }
}