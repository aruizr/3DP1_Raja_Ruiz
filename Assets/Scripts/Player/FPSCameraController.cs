using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private float sensitivity;
    [SerializeField] private float smoothing;
    [SerializeField] private Transform pitchController;
    [SerializeField] private Range<float> pitchRange;
    [SerializeField] private float recoilIntensity;
    [SerializeField] private float recoilTime;

    private Vector2 _input, _targetRotation, _currentRotation, _offset;

    private void Awake()
    {
        _input = _targetRotation = _currentRotation = _offset = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _targetRotation += (_input + _offset) * (sensitivity * Time.deltaTime);
        _targetRotation.y = Mathf.Clamp(_targetRotation.y, pitchRange.min, pitchRange.max);
        _currentRotation = Vector2.Lerp(_currentRotation, _targetRotation, smoothing);
        transform.localEulerAngles = Vector3.up * _currentRotation.x;
        pitchController.localEulerAngles = Vector3.right * _currentRotation.y;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onShoot, OnShootEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onShoot, OnShootEvent);
    }

    private void OnShootEvent(Dictionary<string, object> obj)
    {
        _offset = new Vector2(Random.Range(-recoilIntensity, recoilIntensity),
            Random.Range(-recoilIntensity, recoilIntensity));
        DOTween.To(
            () => _offset,
            value => _offset = value,
            Vector2.zero,
            recoilTime
        );
    }

    public void OnLook(InputValue inputValue)
    {
        _input = inputValue.Get<Vector2>();
    }
}