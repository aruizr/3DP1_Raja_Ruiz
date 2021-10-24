using System;
using System.Collections.Generic;
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
        EventManager.StartListening(EventData.Instance.onShoot, OnShootEvent);
        EventManager.StartListening(EventData.Instance.onReload, OnReloadEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onShoot, OnShootEvent);
        EventManager.StartListening(EventData.Instance.onReload, OnReloadEvent);
    }

    private void OnReloadEvent(Dictionary<string, object> obj)
    {
        _gunRecoilSequence.Restart();
        _gunRecoilSequence.Pause();
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
        _gunRecoilSequence.Join(
            Camera.main.DOShakePosition(0.2f, new Vector3(0.05f, 0.05f, 0), 20));
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

    private void OnShootEvent(Dictionary<string, object> message)
    {
        _gunRecoilSequence.Restart();
    }

    [Serializable]
    private struct RecoilSettings
    {
        public float rotation, movement, startDuration, endDuration;
    }
}