using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty
    {
        easy, 
        medium, 
        hard
    }

    private Difficulty _currentDifficulty;
    private int _currentCopAmount;
    private int _currentNpcAmount;
    private int _copsToDelete;
    private int _npcsToDelete;
    private float _currentCommonWeight;
    private float _currentRareWeight;
    private float _currentSuperRareWeight;

    
    [Header("Easy Difficulty")]
    [SerializeField] private int EasyCopAmount;
    [SerializeField] private int EasyNpcAmount;
    [SerializeField] private float EasyCommonWeight;
    [SerializeField] private float EasyRareWeight;
    [SerializeField] private float EasySuperRareWeight;
    
    
    [Header("Medium Difficulty")]
    [SerializeField] private int MediumCopAmount;
    [SerializeField] private int MediumNpcAmount;
    [SerializeField] private float MediumCommonWeight;
    [SerializeField] private float MediumRareWeight;
    [SerializeField] private float MediumSuperRareWeight;
    
    [Header("Hard Difficulty")]
    [SerializeField] private int HardCopAmount;
    [SerializeField] private int HardNpcAmount;
    [SerializeField] private float HardCommonWeight;
    [SerializeField] private float HardRareWeight;
    [SerializeField] private float HardSuperRareWeight;
    
    public static DifficultyManager Instance {get; private set;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetEasyDifficulty()
    {
        _currentDifficulty = Difficulty.easy;
    }
    
    public void SetMedDifficulty()
    {
        _currentDifficulty = Difficulty.medium;
    }
    
    public void SetHardDifficulty()
    {
        _currentDifficulty = Difficulty.hard;
    }

    public Difficulty GetDifficulty()
    {
        return _currentDifficulty;
    }

    public void ApplyDifficulty()
    {
        CopController[] cops = FindObjectsByType<CopController>(FindObjectsSortMode.None);
        PasserbyController[] npcs = FindObjectsByType<PasserbyController>(FindObjectsSortMode.None);

        List<CopController> copsList = new List<CopController>(cops);
        List<PasserbyController> npcsList = new List<PasserbyController>(npcs);

        _currentCopAmount = copsList.Count;
        _currentNpcAmount = npcsList.Count;

        switch (_currentDifficulty)
        {
            case Difficulty.easy:
                _copsToDelete = Math.Max(_currentCopAmount - EasyCopAmount, 0);
                _npcsToDelete = Math.Max(_currentNpcAmount - EasyNpcAmount, 0);
                _currentCommonWeight = EasyCommonWeight;
                _currentRareWeight = EasyRareWeight;
                _currentSuperRareWeight = EasySuperRareWeight;
                break;
            case Difficulty.medium:
                _copsToDelete = Math.Max(_currentCopAmount - MediumCopAmount, 0);
                _npcsToDelete = Math.Max(_currentNpcAmount - HardCopAmount, 0);
                _currentCommonWeight = MediumCommonWeight;
                _currentRareWeight = MediumRareWeight;
                _currentSuperRareWeight = MediumSuperRareWeight;
                break;
            case Difficulty.hard:
                _copsToDelete = Math.Max(_currentCopAmount - HardCopAmount, 0);
                _npcsToDelete = Math.Max(_currentNpcAmount - HardNpcAmount, 0);
                _currentCommonWeight = HardCommonWeight;
                _currentRareWeight = HardRareWeight;
                _currentSuperRareWeight = HardSuperRareWeight;
                break;
            default:
                _copsToDelete = Math.Max(_currentCopAmount - EasyCopAmount, 0);
                _npcsToDelete = Math.Max(_currentNpcAmount - EasyNpcAmount, 0);
                _currentCommonWeight = EasyCommonWeight;
                _currentRareWeight = EasyRareWeight;
                _currentSuperRareWeight = EasySuperRareWeight;
                break;
        }

        for (int i = 0; i < _copsToDelete; i++)
        {
            int index = Random.Range(0, copsList.Count);
            if (copsList.Count == 0) break;
            Destroy(copsList[index].gameObject);
            copsList.RemoveAt(index);
        }

        for (int i = 0; i < _npcsToDelete; i++)
        {
            int index = Random.Range(0, npcsList.Count);
            if (npcsList.Count == 0) break;
            Destroy(npcsList[index].gameObject);
            npcsList.RemoveAt(index);
        }
        
        FindObjectOfType<LootRandom>().SetWeights
            (_currentCommonWeight, 
            _currentRareWeight, 
            _currentSuperRareWeight);
    }
}
