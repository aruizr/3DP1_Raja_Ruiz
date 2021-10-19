using UnityEngine;

public class HealthItem : MonoBehaviour, IInteractiveItem
{
    public string GetName()
    {
        return "Health Kit";
    }

    public string GetDescription()
    {
        return "";
    }

    public void Interact()
    {
        EventManager.TriggerEvent(EventData.instance.onRestorePlayerHealth);
        Destroy(gameObject);
    }
}