using UnityEngine;
using UnityEngine.InputSystem;

public class FPSAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private readonly int idle = Animator.StringToHash("idle");
    private readonly int jumping = Animator.StringToHash("jump loop");
    private readonly int walkBackward = Animator.StringToHash("walk backward");
    private readonly int walkForward = Animator.StringToHash("walk forward");
    private readonly int walkLeft = Animator.StringToHash("walk left");
    private readonly int walkRight = Animator.StringToHash("walk right");

    private float _crossFadeTime = 0.25f;
    private Vector2 _direction;
    private StateMachine<State> _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine<State>();

        _stateMachine.OnStatePhase(State.Idle, StatePhase.Enter, () => PlayAnimation(idle));
        _stateMachine.OnStatePhase(State.WalkForward, StatePhase.Enter, () => PlayAnimation(walkForward));
        _stateMachine.OnStatePhase(State.WalkRight, StatePhase.Enter, () => PlayAnimation(walkRight));
        _stateMachine.OnStatePhase(State.WalkLeft, StatePhase.Enter, () => PlayAnimation(walkLeft));
        _stateMachine.OnStatePhase(State.WalkBackward, StatePhase.Enter, () => PlayAnimation(walkBackward));
        _stateMachine.OnStatePhase(State.Jumping, StatePhase.Enter, () => PlayAnimation(jumping));

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

    private enum State
    {
        Idle,
        WalkForward,
        WalkRight,
        WalkLeft,
        WalkBackward,
        Jumping
    }
}