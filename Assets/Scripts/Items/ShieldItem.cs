public class ShieldItem : Item
{
    public override void Interact()
    {
        EventManager.TriggerEvent(EventData.Instance.onRestorePlayerShield, null);
        Destroy(gameObject);
    }
}