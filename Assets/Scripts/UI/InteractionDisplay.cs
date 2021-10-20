using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onUpdateInteractionTarget, OnDisplayInteractionInfo);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onUpdateInteractionTarget, OnDisplayInteractionInfo);
    }

    private void OnDisplayInteractionInfo(Dictionary<string, object> message)
    {
        var target = (GameObject) message["target"];
        var interactiveItem = target?.GetComponent<IInteractiveItem>();

        interactionText.text = interactiveItem != null
            ? "Press <style=\"C1\">E</style> to pick up " + interactiveItem.GetName()
            : null;
    }
}