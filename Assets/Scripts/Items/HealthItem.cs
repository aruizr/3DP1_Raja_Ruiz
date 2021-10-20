public class HealthItem : Item
{
    public override void Interact()
    {
        EventManager.TriggerEvent(EventData.instance.onRestorePlayerHealth, null);
        Destroy(gameObject);
    }
}