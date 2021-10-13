using System;
using System.Collections.Generic;
using System.Linq;

public enum StatePhase
{
    Enter,
    Stay,
    Exit
}

public class StateMachine<T> where T : Enum
{
    private readonly Dictionary<T, Dictionary<StatePhase, Action>> _actions;
    private T _currentState;
    private bool _isLocked;

    public StateMachine()
    {
        _isLocked = false;
        _actions = new Dictionary<T, Dictionary<StatePhase, Action>>();
        foreach (T state in Enum.GetValues(typeof(T)))
            _actions.Add(state, Enum
                .GetValues(typeof(StatePhase))
                .Cast<StatePhase>()
                .ToDictionary<StatePhase, StatePhase, Action>(phase => phase, phase => null));
    }

    public T CurrentState
    {
        get => _currentState;
        set
        {
            if (_isLocked) return;
            if (_currentState.Equals(value)) return;
            _actions[_currentState][StatePhase.Exit]?.Invoke();
            _currentState = value;
            _actions[_currentState][StatePhase.Enter]?.Invoke();
        }
    }

    public void Lock() => _isLocked = true;

    public void Unlock() => _isLocked = false;

    public void OnStatePhase(T state, StatePhase phase, Action action) => _actions[state][phase] = action;

    public void Update() => _actions[_currentState][StatePhase.Stay]?.Invoke();
}