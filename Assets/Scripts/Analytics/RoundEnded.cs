using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEnded : Unity.Services.Analytics.Event
{
    public RoundEnded() : base("Round_Ended")
    {
        
    }

    //public string PlayerPositions {set {SetParameter("Player_positions", value);}}
    //public string PosWhenDetected {set {SetParameter("Pos_when_detected", value);}}
    //public string PosWhenEscaped {set {SetParameter("Pos_when_escaped", value);}}
    //public string QualityLooted {set {SetParameter("Quality_looted", value);}}
    //public string WeightsWhenLooted {set {SetParameter("Weights_when_looted", value);}}
    public int TimesDetected {set {SetParameter("Time_Detected", value);}}
    public int TimesLooted {set {SetParameter("Times_looted", value);}}
    public int TimesEscaped {set {SetParameter("Times_escaped", value);}}
    //public float TimeWhenLooted {set {SetParameter("Time_when_looted", value);}}
}
