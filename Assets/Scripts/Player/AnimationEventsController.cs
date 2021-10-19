using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    public void OnFinishReload()
    {
        EventManager.TriggerEvent(EventData.instance.onReloadFinishEventName);
    }

    public void OnStartReload()
    {
        EventManager.TriggerEvent(EventData.instance.onReloadStartEventName);
    }
}