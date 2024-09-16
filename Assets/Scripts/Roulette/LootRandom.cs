using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LootRandom : MonoBehaviour
{
    public DataBase dataBase;
    public LootView view;
    public List<RarityInfo> infos;
    Dictionary<RarirtyEnum, float> _items;

    private void Awake()
    {
        _items = new Dictionary<RarirtyEnum, float>();
        for (int i = 0; i < infos.Count; i++)
        {
            _items[infos[i].type] = infos[i].weight;
        }
    }

    public void GetRandomCard()
    {
        RarirtyEnum rarity = MyRandoms.Roulette(_items);
        SetCard(rarity);
    }

    void SetCard(RarirtyEnum rarity)
    {
        if (!dataBase) return;
        if (!dataBase.cards.ContainsKey(rarity)) return;
        Debug.Log(rarity);
        Sprite[] cards = dataBase.cards[rarity];
        int random = UnityEngine.Random.Range(0,cards.Length); 
        view.SetLoot(rarity, cards[random]);
        view.AppearLoot();
    }
}