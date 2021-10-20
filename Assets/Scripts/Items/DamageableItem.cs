using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableItem : HealthSystem
{
    protected override void OnDie()
    {
        EventManager.TriggerEvent(EventData.instance.onItemDestroyed, new Dictionary<string, object>()
        {
            {"item", gameObject}
        });
        gameObject.SetActive(false);
    }
}
