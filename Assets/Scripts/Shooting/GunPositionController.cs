using System.Collections.Generic;
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
        EventManager.StartListening(EventData.Instance.onReloadStart, OnReloadEvent);
        EventManager.StartListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onReloadStart, OnReloadEvent);
        EventManager.StopListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnReloadFinish(Dictionary<string, object> message)
    {
        gunTransform.parent = _originalGunParent;
        gunTransform.localPosition = _originalGunLocalPosition;
        gunTransform.localEulerAngles = _originalGunLocalRotation;
    }

    private void OnReloadEvent(Dictionary<string, object> message)
    {
        gunTransform.parent = handTransform;
    }
}