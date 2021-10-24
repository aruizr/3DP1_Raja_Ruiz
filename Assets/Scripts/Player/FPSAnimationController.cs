using System.Collections.Generic;
using UnityEngine;

public class FPSAnimationController : ExtendedMonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    private readonly int _idleAnimation = Animator.StringToHash("idle");
    private readonly int _jumpingAnimation = Animator.StringToHash("jump loop");
    private readonly int _reloadingAnimation = Animator.StringToHash("idle reload");
    private readonly int _walkBackwardAnimation = Animator.StringToHash("walk backward");
    private readonly int _walkBackwardLeftAnimation = Animator.StringToHash("walk backward left");
    private readonly int _walkBackwardRightAnimation = Animator.StringToHash("walk backward right");
    private readonly int _walkForwardAnimation = Animator.StringToHash("walk forward");
    private readonly int _walkForwardLeftAnimation = Animator.StringToHash("walk forward left");
    private readonly int _walkForwardRightAnimation = Animator.StringToHash("walk forward right");
    private readonly int _walkLeftAnimation = Animator.StringToHash("walk left");
    private readonly int _walkRightAnimation = Animator.StringToHash("walk right");
    private float _crossFadeTime = 0.25f;
    private StateMachine<State> _stateMachine;

    private void Awake()
    {
        _stateMachine = new StateMachine<State>();

        _stateMachine.OnStatePhase(State.Idle, StatePhase.Enter, () => PlayAnimation(_idleAnimation));

        _stateMachine.OnStatePhase(State.WalkForward, StatePhase.Enter, () => PlayAnimation(_walkForwardAnimation));
        _stateMachine.OnStatePhase(State.WalkRight, StatePhase.Enter, () => PlayAnimation(_walkRightAnimation));
        _stateMachine.OnStatePhase(State.WalkLeft, StatePhase.Enter, () => PlayAnimation(_walkLeftAnimation));
        _stateMachine.OnStatePhase(State.WalkBackward, StatePhase.Enter, () => PlayAnimation(_walkBackwardAnimation));

        _stateMachine.OnStatePhase(State.WalkForwardRight, StatePhase.Enter,
            () => PlayAnimation(_walkForwardRightAnimation));
        _stateMachine.OnStatePhase(State.WalkForwardLeft, StatePhase.Enter,
            () => PlayAnimation(_walkForwardLeftAnimation));
        _stateMachine.OnStatePhase(State.WalkBackwardRight, StatePhase.Enter,
            () => PlayAnimation(_walkBackwardRightAnimation));
        _stateMachine.OnStatePhase(State.WalkBackwardLeft, StatePhase.Enter,
            () => PlayAnimation(_walkBackwardLeftAnimation));

        _stateMachine.OnStatePhase(State.Jumping, StatePhase.Enter, () => PlayAnimation(_jumpingAnimation));
        _stateMachine.OnStatePhase(State.Reloading, StatePhase.Enter, () => PlayAnimation(_reloadingAnimation));

        _stateMachine.CurrentState = State.Idle;
    }

    private void Update()
    {
        _crossFadeTime = _stateMachine.CurrentState == State.Jumping ? 0.05f : 0.2f;
        _stateMachine.CurrentState = GetNextState();
        _stateMachine.Update();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onReload, OnReloadStart);
        EventManager.StartListening(EventData.Instance.onReloadFinish, OnReloadFinish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onReload, OnReloadStart);
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

    private State GetNextState()
    {
        var velocity = transform.InverseTransformDirection(controller.velocity);

        return controller.isGrounded switch
        {
            false => State.Jumping,
            _ => (velocity.z > .1f) switch
            {
                true when velocity.x > .1f => State.WalkForwardRight,
                true when velocity.x < -.1f => State.WalkForwardLeft,
                true => State.WalkForward,
                _ => (velocity.z < -.1f) switch
                {
                    true when velocity.x > .1f => State.WalkBackwardRight,
                    true when velocity.x < -.1f => State.WalkBackwardLeft,
                    true => State.WalkBackward,
                    _ => (velocity.x > .1f) switch
                    {
                        false when velocity.x < -.1f => State.WalkLeft,
                        true => State.WalkRight,
                        _ => State.Idle
                    }
                }
            }
        };
    }

    private void OnReloadStart(Dictionary<string, object> dictionary)
    {
        _stateMachine.CurrentState = State.Reloading;
        _stateMachine.Lock();
    }

    private enum State
    {
        Idle,
        WalkForward,
        WalkForwardRight,
        WalkForwardLeft,
        WalkRight,
        WalkLeft,
        WalkBackward,
        WalkBackwardRight,
        WalkBackwardLeft,
        Jumping,
        Reloading
    }
}