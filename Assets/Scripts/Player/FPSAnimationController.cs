using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private readonly int _idleAnimation = Animator.StringToHash("idle");
    private readonly int _jumpingAnimation = Animator.StringToHash("jump loop");
    private readonly int _reloadingAnimation = Animator.StringToHash("idle reload");
    private readonly int _walkBackwardAnimation = Animator.StringToHash("walk backward");
    private readonly int _walkForwardAnimation = Animator.StringToHash("walk forward");
    private readonly int _walkLeftAnimation = Animator.StringToHash("walk left");
    private readonly int _walkRightAnimation = Animator.StringToHash("walk right");

    private float _crossFadeTime = 0.25f;
    private Vector2 _direction;
    private StateMachine<State> _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine<State>();

        _stateMachine.OnStatePhase(State.Idle, StatePhase.Enter, () => PlayAnimation(_idleAnimation));
        _stateMachine.OnStatePhase(State.WalkForward, StatePhase.Enter, () => PlayAnimation(_walkForwardAnimation));
        _stateMachine.OnStatePhase(State.WalkRight, StatePhase.Enter, () => PlayAnimation(_walkRightAnimation));
        _stateMachine.OnStatePhase(State.WalkLeft, StatePhase.Enter, () => PlayAnimation(_walkLeftAnimation));
        _stateMachine.OnStatePhase(State.WalkBackward, StatePhase.Enter, () => PlayAnimation(_walkBackwardAnimation));
        _stateMachine.OnStatePhase(State.Jumping, StatePhase.Enter, () => PlayAnimation(_jumpingAnimation));
        _stateMachine.OnStatePhase(State.Reloading, StatePhase.Enter, () => PlayAnimation(_reloadingAnimation));

        _stateMachine.CurrentState = State.Idle;
    }

    private void Update()
    {
        _crossFadeTime = _stateMachine.CurrentState == State.Jumping ? 0.05f : 0.2f;
        _stateMachine.CurrentState = controller.isGrounded switch
        {
            false when _stateMachine.CurrentState != State.Jumping => State.Jumping,
            true when _stateMachine.CurrentState == State.Jumping => GetCurrentState(),
            _ => _stateMachine.CurrentState
        };

        _stateMachine.Update();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnReloadFinish(Dictionary<string, object> message)
    {
        _stateMachine.Unlock();
    }

    private void PlayAnimation(int anim)
    {
        animator.CrossFade(anim, _crossFadeTime);
    }

    private State GetCurrentState()
    {
        return (_direction.x, _direction.y) switch
        {
            (0, 1) => State.WalkForward,
            (0, -1) => State.WalkBackward,
            (1, 0) => State.WalkRight,
            (-1, 0) => State.WalkLeft,
            _ => State.Idle
        };
    }

    private void OnMove(InputValue inputValue)
    {
        if (!controller.isGrounded) return;
        _direction = inputValue.Get<Vector2>();
        _stateMachine.CurrentState = GetCurrentState();
    }

    private void OnReload()
    {
        _stateMachine.CurrentState = State.Reloading;
        _stateMachine.Lock();
    }

    private enum State
    {
        Idle,
        WalkForward,
        WalkRight,
        WalkLeft,
        WalkBackward,
        Jumping,
        Reloading
    }
}