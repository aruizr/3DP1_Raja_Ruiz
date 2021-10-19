using UnityEngine;

public class GunPositionController : ExtendedMonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform handTransform;

    private Vector3 _originalGunLocalPosition, _originalGunLocalRotation;
    private Transform _originalGunParent;

    private void Awake()
    {
        _originalGunParent = gunTransform.parent;
        _originalGunLocalPosition = gunTransform.localPosition;
        _originalGunLocalRotation = gunTransform.localEulerAngles;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onReloadStartEventName, OnReloadEvent);
        EventManager.StartListening(EventData.instance.onReloadFinishEventName, OnReloadFinishEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onReloadStartEventName, OnReloadEvent);
        EventManager.StopListening(EventData.instance.onReloadFinishEventName, OnReloadFinishEvent);
    }

    private void OnReloadFinishEvent(object[] args)
    {
        gunTransform.parent = _originalGunParent;
        gunTransform.localPosition = _originalGunLocalPosition;
        gunTransform.localEulerAngles = _originalGunLocalRotation;
    }

    private void OnReloadEvent(object[] args)
    {
        gunTransform.parent = handTransform;
    }
}