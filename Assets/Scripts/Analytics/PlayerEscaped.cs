using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEscaped : Unity.Services.Analytics.Event
{
    public PlayerEscaped() : base("Player_Escaped")
    {

    }
    
    public float PosXWhenDetected {set {SetParameter("Pos_X_when_detected", value);}}
    public float PosYWhenDetected {set {SetParameter("Pos_Y_when_detected", value);}}
    public float PosXWhenEscaped {set {SetParameter("Pos_X_when_escaped", value);}}
    public float PosYWhenEscaped {set {SetParameter("Pos_Y_when_escaped", value);}}
    public float TimeSpentEscaping {set {SetParameter("Time_spent_escaping", value);}}
}
