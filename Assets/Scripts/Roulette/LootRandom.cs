using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro; 

public class LootRandom : MonoBehaviour
{
    public DataBase dataBase;
    public List<RarityInfo> infos;
    public Transform spawnPoint;  
    public TextMeshProUGUI timerText;  

    Dictionary<RarirtyEnum, float> _items;
    Dictionary<RarirtyEnum, float> _baseWeights;

    private float _averageTime = 90f;
    private float _timeElapsed;             
    private bool _timerActive = false;      
    private PlayerInventory _inventory;
    private GameManager _gameManager;

    private float _currentCommonWeight;
    private float _currentRareWeight;
    private float _currentSuperWeight;

    private void Awake()
    {
        _baseWeights = new Dictionary<RarirtyEnum, float>
        {
            { RarirtyEnum.C, 10f },  
            { RarirtyEnum.R, 5f },  
            { RarirtyEnum.SR, 1f }  
        };
        
        _items = new Dictionary<RarirtyEnum, float>(_baseWeights);
        _inventory = FindObjectOfType<PlayerInventory>();  
        ResetWeights();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (_inventory && !_inventory._hasItem && !_timerActive)
        {
            _timerActive = true;
            _timeElapsed = 0f;
            ResetWeights();
        }

        if (_timerActive && _inventory._hasItem)
        {
            _timerActive = false;
            AdjustWeightsBasedOnTime(_timeElapsed);
        }

        if (_timerActive)
        {
            _timeElapsed += Time.deltaTime;
            UpdateTimerUI();  
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timeElapsed);
            timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
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

        int itemValue = 0;
        if (lootable != null)
        {
            itemValue = infos.Find(info => info.type == rarity).value;
            lootable.SetItemValue(itemValue);
        }
        
        _gameManager.Looted(_currentCommonWeight, _currentRareWeight, _currentSuperWeight, _timeElapsed, itemValue);

        return spawnedItem;
    }

    private void AdjustWeightsBasedOnTime(float timeTaken)
    {
        float speedFactor = Mathf.Clamp(_averageTime / timeTaken, 0.1f, 2f);

        foreach (var rarity in _items.Keys)
        {
            float baseWeight = _baseWeights[rarity];

            if (rarity == RarirtyEnum.SR)//speedFactor = 0.5
            {
                _currentCommonWeight = Mathf.Lerp(baseWeight * 0.5f, baseWeight * 10f, speedFactor); //100
                _items[rarity] = _currentCommonWeight; 
            }
            else if (rarity == RarirtyEnum.R)
            {
                _currentRareWeight= Mathf.Lerp(baseWeight * 0.75f, baseWeight * 2f, speedFactor); //50
                _items[rarity] = _currentRareWeight;

            }
            else if (rarity == RarirtyEnum.C)
            {
                _currentSuperWeight = Mathf.Lerp(baseWeight * 2f, baseWeight * 0.1f, speedFactor); //25
                _items[rarity] = _currentSuperWeight;
            }
        }
    }

    private void ResetWeights()
    {
        _items = new Dictionary<RarirtyEnum, float>(_baseWeights);
    }

    public void SetWeights(float commonWeight, float rareWeight, float superRareWeight)
    {
        _baseWeights = new Dictionary<RarirtyEnum, float>
        {
            { RarirtyEnum.C, commonWeight },  
            { RarirtyEnum.R, rareWeight },  
            { RarirtyEnum.SR, superRareWeight }  
        };

        _items = new Dictionary<RarirtyEnum, float>(_baseWeights);
        ResetWeights();
    }
}