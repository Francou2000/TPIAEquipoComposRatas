using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : Unity.Services.Analytics.Event
{
    public PlayerMoving() : base("Player_Moving")
    {

    }
    
    public float PlayerPosX {set {SetParameter("Player_position_X", value);}}
    public float PlayerPosY {set {SetParameter("Player_position_Y", value);}}
    public string Difficulty {set {SetParameter("Difficulty", value);}}
}
