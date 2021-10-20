using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    public void OnFinishReload()
    {
        EventManager.TriggerEvent(EventData.instance.onReloadFinish, null);
    }

    public void OnStartReload()
    {
        EventManager.TriggerEvent(EventData.instance.onReloadStart, null);
    }
}