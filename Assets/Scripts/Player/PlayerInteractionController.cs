using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private float range;

    private GameObject _currentInteractionTarget;

    private GameObject CurrentInteractionTarget
    {
        get => _currentInteractionTarget;
        set
        {
            if (!_currentInteractionTarget && !value) return;
            if (_currentInteractionTarget && value && _currentInteractionTarget.Equals(value)) return;
            _currentInteractionTarget = value;
            NotifyInteractionTarget();
        }
    }

    private void Update()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        CurrentInteractionTarget = Physics.Raycast(ray, out var hit, range) ? hit.collider.gameObject : null;
    }

    private void NotifyInteractionTarget()
    {
        EventManager.TriggerEvent(EventData.Instance.onUpdateInteractionTarget, new Dictionary<string, object>
        {
            {"target", CurrentInteractionTarget}
        });
    }

    private void OnInteract()
    {
        if (!CurrentInteractionTarget) return;
        var item = CurrentInteractionTarget.GetComponent<IInteractiveItem>();
        item?.Interact();
    }
}