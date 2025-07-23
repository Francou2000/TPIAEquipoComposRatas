using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDied : Unity.Services.Analytics.Event
{
    public PlayerDied() : base("Player_Died")
    {
        
    }

    public float PosXWhenDetected {set {SetParameter("Pos_X_when_detected", value);}}
    public float PosYWhenDetected {set {SetParameter("Pos_Y_when_detected", value);}}
    public float DeathPosX {set {SetParameter("Death_pos_X", value);}}
    public float DeathPosY {set {SetParameter("Death_pos_Y", value);}}
    public string Difficulty {set {SetParameter("Difficulty", value);}}
    public string KillerID {set {SetParameter("Killer_ID", value);}}
}
