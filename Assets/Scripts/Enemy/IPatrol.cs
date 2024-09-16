using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrol
{
    Transform PatrolPointA { get; set; }
    Transform PatrolPointB { get; set; }
    Transform CurrentTarget { get; set; }
    bool HasArrived { get; set; }
}
