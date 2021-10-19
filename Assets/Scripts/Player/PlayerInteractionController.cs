using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private float range;
    
    private GameObject _currentInteractionTarget;

    private void Start()
    {
        _currentInteractionTarget = GetCurrentInteractionTarget();
    }

    private void Update()
    {
        var target = GetCurrentInteractionTarget();
        if (!target && !_currentInteractionTarget) return;
        if (target && _currentInteractionTarget && target.Equals(_currentInteractionTarget)) return;
        _currentInteractionTarget = target;
        EventManager.TriggerEvent(EventData.instance.onDisplayInteractionInfo, _currentInteractionTarget);
    }

    private GameObject GetCurrentInteractionTarget()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return Physics.Raycast(ray, out var hit, range) ? hit.collider.gameObject : null;
    }

    private void OnInteract()
    {
        if (!_currentInteractionTarget) return;
        var item = _currentInteractionTarget.GetComponent<IInteractiveItem>();
        item?.Interact();
    }
}