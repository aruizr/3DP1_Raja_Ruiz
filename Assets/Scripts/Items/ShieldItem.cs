public class ShieldItem : Item
{
    public override void Interact()
    {
        EventManager.TriggerEvent(EventData.instance.onRestorePlayerShield, null);
        Destroy(gameObject);
    }
}