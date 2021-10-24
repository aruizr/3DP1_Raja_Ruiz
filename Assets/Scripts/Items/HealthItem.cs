public class HealthItem : Item
{
    public override void Interact()
    {
        EventManager.TriggerEvent(EventData.Instance.onRestorePlayerHealth, null);
        Destroy(gameObject);
    }
}