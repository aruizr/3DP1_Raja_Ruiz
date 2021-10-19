using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCameraController : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private float sensitivity;
    [SerializeField] private float smoothing;
    [SerializeField] private Transform pitchController;
    [SerializeField] private Range<float> pitchRange;

    private Vector2 _input, _targetRotation, _currentRotation;

    private void Awake()
    {
        _input = _targetRotation = _currentRotation = Vector2.zero;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        _targetRotation += _input * (sensitivity * Time.deltaTime);
        _targetRotation.y = Mathf.Clamp(_targetRotation.y, pitchRange.min, pitchRange.max);
        _currentRotation = Vector2.Lerp(_currentRotation, _targetRotation, smoothing);
        transform.localEulerAngles = Vector3.up * _currentRotation.x;
        pitchController.localEulerAngles = Vector3.right * _currentRotation.y;
    }

    public void OnLook(InputValue inputValue)
    {
        _input = inputValue.Get<Vector2>();
    }
}