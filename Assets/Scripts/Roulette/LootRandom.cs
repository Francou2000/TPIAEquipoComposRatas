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
    Dictionary<RarirtyEnum, float> _baseWeights;

    private float _averageTime = 90f;

    private float _timeElapsed;             
    private bool _timerActive = false;      
    private PlayerInventory _inventory;    

    private void Awake()
    {
        _baseWeights = new Dictionary<RarirtyEnum, float>
    {
        { RarirtyEnum.C, 10f },  
        { RarirtyEnum.R, 5f },  
        { RarirtyEnum.SR, 1f }  
    };

        _items = new Dictionary<RarirtyEnum, float>(_baseWeights);

        _items = new Dictionary<RarirtyEnum, float>();
        _inventory = FindObjectOfType<PlayerInventory>();  
        ResetWeights();
    }

    private void Update()
    {
        if (_inventory && !_inventory._hasItem && !_timerActive)
        {
            _timerActive = true;
            _timeElapsed = 0f;
        }

        if (_timerActive && _inventory._hasItem)
        {
            _timerActive = false;
            AdjustWeightsBasedOnTime(_timeElapsed);
        }

        if (_timerActive)
        {
            _timeElapsed += Time.deltaTime;
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
    private void AdjustWeightsBasedOnTime(float timeTaken)
    {
        float speedFactor = Mathf.Clamp(_averageTime / timeTaken, 0.1f, 2f);

        foreach (var rarity in _items.Keys)
        {
            float baseWeight = _baseWeights[rarity];

            if (rarity == RarirtyEnum.SR)
            {
                _items[rarity] = Mathf.Lerp(baseWeight * 0.5f, baseWeight * 2f, speedFactor);
            }
            else if (rarity == RarirtyEnum.R)
            {
                _items[rarity] = Mathf.Lerp(baseWeight * 0.75f, baseWeight * 1.5f, speedFactor);
            }
            else if (rarity == RarirtyEnum.C)
            {
                _items[rarity] = Mathf.Lerp(baseWeight * 2f, baseWeight * 0.5f, speedFactor);
            }
        }
    }

    private void ResetWeights()
    {
        _items = new Dictionary<RarirtyEnum, float>(_baseWeights);
    }
}