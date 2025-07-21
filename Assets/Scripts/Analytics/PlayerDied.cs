using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDied : Unity.Services.Analytics.Event
{
    public PlayerDied() : base("Player_Died")
    {
        
    }

    public string DeathPos {set {SetParameter("Death_pos", value);}}
    public string Difficulty {set {SetParameter("Difficulty", value);}}
    public string KillerID{set {SetParameter("Killer_ID", value);}}
}
