using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPoints : MonoBehaviour
{
    public int playerPoints = 0;           
    public TextMeshProUGUI pointsText;    

    void Start()
    {
        UpdatePointsUI();
    }

    public void AddPoints(int points)
    {
        playerPoints += points;
        UpdatePointsUI();
    }

    void UpdatePointsUI()
    {
        pointsText.text = "Money: " + playerPoints.ToString();
    }
}

