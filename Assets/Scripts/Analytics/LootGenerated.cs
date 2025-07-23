using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootGenerated : Unity.Services.Analytics.Event
{
    public LootGenerated() : base("LOOT_GENERATED")
    {
        
    }

    public float CommonWeightWhenLooted {set {SetParameter("Common_weight_when_looted", value);}}
    public float RareWeightWhenLooted {set {SetParameter("Rare_weight_when_looted", value);}}
    public float SuperrareWeightWhenLooted {set {SetParameter("Superrare_weight_when_looted", value);}}
    public float TimeWhenLooted {set {SetParameter("Time_when_looted", value);}}
    public string QualityLooted {set {SetParameter("Quality_looted", value);}}
    public string Difficulty {set {SetParameter("Difficulty", value);}}
}
