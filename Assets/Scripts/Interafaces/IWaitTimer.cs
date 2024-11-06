using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWaitTimer
{
    float WaitTimer { get; set; }
    float WaitTime { get; set; }
    bool DoneWaiting { get; set; }
}