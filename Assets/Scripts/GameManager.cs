using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int _deathAmount;
    private string _difficulty;

    private float _latestDetectedPosX;
    private float _latestDetectedPosZ;
    private float _latestEscapedPosX;
    private float _latestEscapedPosZ;
    private bool _isDetected = false;
    private float _timeDetected;
    private int _timesDetected;
    private int _timesEscaped;
    private int _timesLooted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        /*if (DifficultyManager.Instance)
        {
            DifficultyManager.Instance.ApplyDifficulty();
        }*/

        if (DifficultyManager.Instance)
        {
            _difficulty = DifficultyManager.Instance.GetDifficulty().ToString();
        }
        else
        {
            _difficulty = "hard";
        }
    }

    private void Update()
    {
        if (_isDetected)
        {
            _timeDetected += Time.deltaTime;
        }
    }

    public void UpdateDifficulty(string difficulty)
    {
        _difficulty = difficulty;
    }

    public void Death(float posX, float posY, string id)
    {
        _deathAmount++;
        PlayerDied playerDied = new PlayerDied
        {
            PosXWhenDetected = _latestDetectedPosX,
            PosYWhenDetected = _latestDetectedPosZ,
            DeathPosX = posX,
            DeathPosY = posY,
            Difficulty = _difficulty,
            KillerID = id,
        };
        Debug.Log("Death event recorded");
        AnalyticsService.Instance.RecordEvent(playerDied);
        _latestDetectedPosX = 0;
        _latestDetectedPosZ = 0;
    }

    public void Victory(int interactions)
    {
        PlayerWin playerWin = new PlayerWin
        {
            DeathAmount = _deathAmount,
            Difficulty = _difficulty,
            SafeInteractions = interactions,
        };
        Debug.Log("Victory event recorded");
        AnalyticsService.Instance.RecordEvent(playerWin);
        _deathAmount = 0;
    }

    public void Escaped()
    {
        PlayerEscaped playerEscaped = new PlayerEscaped
        {
            PosXWhenDetected = _latestDetectedPosX,
            PosYWhenDetected = _latestDetectedPosZ,
            PosXWhenEscaped = _latestEscapedPosX,
            PosYWhenEscaped = _latestEscapedPosZ,
            TimeSpentEscaping = _timeDetected,
        };
        _isDetected = false;
        _timeDetected = 0;
        Debug.Log("Escape event recorded");
        AnalyticsService.Instance.RecordEvent(playerEscaped);
    }

    public void UpdatePosition(float posX, float posY)
    {
        PlayerMoving playerMoving = new PlayerMoving()
        {
            PlayerPosX = posX,
            PlayerPosY = posY,
            Difficulty = _difficulty,
        };
        Debug.Log("Moving player position recorded");
        AnalyticsService.Instance.RecordEvent(playerMoving);
    }

    public void Looted(float Common, float Rare, float Super, float Time, int value)
    {
        string Quality;
        if (value == 25) 
        {
            Quality = "C";
        }  else if (value == 50)
        {
            Quality = "R";
        }
        else
        {
            Quality = "SR";
        }
        _timesLooted++;
        LootGenerated lootGenerated = new LootGenerated
        {
            CommonWeightWhenLooted = Common,
            RareWeightWhenLooted = Rare,
            SuperrareWeightWhenLooted = Super,
            TimeWhenLooted = Time,
            QualityLooted = Quality,
            Difficulty = _difficulty,
        };
        Debug.Log("Looted event recorded");
        AnalyticsService.Instance.RecordEvent(lootGenerated);
    }

    public void GameOver()
    {
        RoundEnded roundEnded = new RoundEnded
        {
            TimesDetected = _timesDetected,
            TimesEscaped = _timesEscaped,
            TimesLooted = _timesLooted,
        };
        Debug.Log("Game over recorded");
        AnalyticsService.Instance.RecordEvent(roundEnded);
        _timesDetected = 0;
        _timesEscaped = 0;
        _timesLooted = 0;
    }

    public void updateLatestDetectedPos(float x, float y)
    {
        //llamado en el switch a EnemyChaseState
        _latestDetectedPosX = x;
        _latestDetectedPosZ = y;
        _timesDetected++;
    }

    public void updateLatestEscapedPos(float x, float y)
    {
        //llamado en la salida del EnemyChaseState
        _latestEscapedPosX = x;
        _latestEscapedPosZ = y;
        _timesEscaped++;
        Debug.Log("Manager confirma escape");
    }

    public void GotDetected()
    {
        //llamado en el switch a EnemyChaseState
        _isDetected = true;
        Debug.Log("Manager confirma detección");
    }

}
