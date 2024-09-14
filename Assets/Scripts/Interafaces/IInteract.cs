using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract 
{
    Action OnInteract { get; set; }

    void Interact();
}
