using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlert
{
    bool IsAlerted { get; set; }
    float AlertedTimer { get; set; }
    float AlertedTime { get; set; }
}
