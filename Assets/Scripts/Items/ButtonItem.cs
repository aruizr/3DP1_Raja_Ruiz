public class ButtonItem : Item
{
    public override void Interact()
    {
        EventManager.TriggerEvent(EventData.Instance.onChallengeStartedByPlayer, null);
    }
}