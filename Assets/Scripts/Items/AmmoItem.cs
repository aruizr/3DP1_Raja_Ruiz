using System.Collections.Generic;

public class AmmoItem : Item
{
    public override void Interact()
    {
        var ammoItemData = (AmmoItemData) itemData;
        var ammoClip = new AmmoClip(ammoItemData.ammoClip.Rounds);
        EventManager.TriggerEvent(EventData.instance.onGivePlayerAmmo, new Dictionary<string, object>
        {
            {"ammoClip", ammoClip}
        });
        Destroy(gameObject);
    }
}