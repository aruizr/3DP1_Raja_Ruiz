using System;
using DG.Tweening;
using UnityEngine;

public class GunRecoilController : MonoBehaviour
{
    [SerializeField] private Transform gunTransform;
    [SerializeField] private RecoilSettings recoilSettings;

    private Sequence _gunRecoilSequence;
    private Vector3 _originalGunLocalPosition;
    private Vector3 _originalGunRotation;

    private void Awake()
    {
        _originalGunRotation = gunTransform.localEulerAngles;
        _originalGunLocalPosition = gunTransform.localPosition;
        InitGunRecoilSequence();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onShootEventName, OnShootEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onShootEventName, OnShootEvent);
    }

    private void InitGunRecoilSequence()
    {
        _gunRecoilSequence = DOTween.Sequence();
        _gunRecoilSequence.Append(
            DOTween.ToAxis(
                () => gunTransform.localEulerAngles,
                value => gunTransform.localEulerAngles = value,
                _originalGunRotation.z + recoilSettings.rotation, recoilSettings.startDuration, AxisConstraint.Z));
        _gunRecoilSequence.Join(
            DOTween.ToAxis(
                () => gunTransform.localPosition,
                value => gunTransform.localPosition = value,
                _originalGunLocalPosition.x + recoilSettings.movement, recoilSettings.startDuration));
        _gunRecoilSequence.Append(
            DOTween.ToAxis(
                () => gunTransform.localEulerAngles,
                value => gunTransform.localEulerAngles = value,
                _originalGunRotation.z, recoilSettings.endDuration, AxisConstraint.Z));
        _gunRecoilSequence.Join(
            DOTween.ToAxis(
                () => gunTransform.localPosition,
                value => gunTransform.localPosition = value,
                _originalGunLocalPosition.x, recoilSettings.endDuration));
        _gunRecoilSequence.SetAutoKill(false);
        _gunRecoilSequence.Pause();
    }

    private void OnShootEvent(object[] args)
    {
        _gunRecoilSequence.Restart();
    }

    [Serializable]
    private struct RecoilSettings
    {
        public float rotation, movement, startDuration, endDuration;
    }
}