using UnityEngine;
using UnityEngine.InputSystem;

public class FPSCharacterController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] [Range(0, 1)] private float smoothing;
    [SerializeField] [Range(0, 3)] private float sprintMultiplier;
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float jumpApexTime = 1;

    private Vector2 _horizontalVelocity, _currentVelocity, _direction;
    private bool _isJumping, _isSprinting;
    private float _verticalVelocity, _jumpSpeed, _gravity;

    private void Awake()
    {
        _horizontalVelocity = Vector2.zero;
        _gravity = 2 * jumpHeight / (jumpApexTime * jumpApexTime);
        _jumpSpeed = 2 * jumpHeight / jumpApexTime;
    }

    private void Update()
    {
        ProcessVerticalVelocity();
        ProcessHorizontalVelocity();

        var t = transform;
        var x = _horizontalVelocity.x * t.right;
        var z = _horizontalVelocity.y * t.forward;
        var y = _verticalVelocity * Vector3.up;

        controller.Move((x + y + z) * Time.deltaTime);
    }

    private void ProcessHorizontalVelocity()
    {
        var targetSpeed = _direction * movementSpeed;
        if (_isSprinting) targetSpeed *= sprintMultiplier;
        _horizontalVelocity = Vector2.SmoothDamp(_horizontalVelocity, targetSpeed, ref _currentVelocity, smoothing);
    }

    private void ProcessVerticalVelocity()
    {
        switch (controller.isGrounded)
        {
            case true when _isJumping:
                _verticalVelocity = _jumpSpeed;
                break;
            case false:
                _verticalVelocity -= _gravity * Time.deltaTime;
                break;
        }
    }

    private void OnMove(InputValue inputValue)
    {
        _direction = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        _isJumping = inputValue.isPressed;
    }

    private void OnSprint(InputValue inputValue)
    {
        _isSprinting = inputValue.isPressed;
    }
}