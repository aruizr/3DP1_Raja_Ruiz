using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractiveItem
{
    string GetName();
    string GetDescription();
    void Interact();
}
