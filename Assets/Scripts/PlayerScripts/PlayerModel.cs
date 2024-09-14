using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Entity, IInteract
{
    Action _onInteract = delegate { };
    public void Interact()
    {
        _onInteract();
    }

    Action IInteract.OnInteract { get => _onInteract; set => _onInteract = value; }
}
