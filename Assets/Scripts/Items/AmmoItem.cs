using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : MonoBehaviour, IInteractiveItem
{
    [SerializeField] private AmmoCartridge _ammoCartridge;

    public string GetName()
    {
        return "Ammo";
    }

    public string GetDescription()
    {
        return _ammoCartridge.Rounds + " rounds";
    }

    public void Interact()
    {
        EventManager.TriggerEvent(EventData.instance.onAddAmmoEventName, _ammoCartridge);
        Destroy(gameObject);
    }
}
