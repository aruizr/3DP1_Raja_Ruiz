using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    public void OnFinishReload()
    {
        EventManager.TriggerEvent(EventData.Instance.onReloadFinish, null);
    }

    public void OnStartReload()
    {
        EventManager.TriggerEvent(EventData.Instance.onReloadStart, null);
    }
}