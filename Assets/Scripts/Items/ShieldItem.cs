using UnityEngine;

public class ShieldItem : MonoBehaviour, IInteractiveItem
{
    public string GetName()
    {
        return "Shield Kit";
    }

    public string GetDescription()
    {
        return "";
    }

    public void Interact()
    {
        EventManager.TriggerEvent(EventData.instance.onRestorePlayerShield);
        Destroy(gameObject);
    }
}