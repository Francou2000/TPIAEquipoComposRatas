using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWin : Unity.Services.Analytics.Event
{
    public PlayerWin() : base("Player_Win")
    {
        
    }

    public int DeathAmount {set {SetParameter("Death_amount", value);}}
    public string Difficulty {set {SetParameter("Difficulty", value);}}
    public int SafeInteractions {set {SetParameter("Safe_interactions", value);}}
}
