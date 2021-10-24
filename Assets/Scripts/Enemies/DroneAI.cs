using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum DroneState
{
    Idle,
    Patrol,
    Alert,
    Chase,
    Attack,
    Hit,
    Die
}

public class DroneAI : HealthSystem
{
    [Space] [SerializeField] private FieldOfView detectionArea;

    [SerializeField] private FieldOfView sightArea;
    [SerializeField] private FieldOfView shootingArea;

    [Space] [Header("Drone Movement")] [SerializeField]
    private float maxPatrolTravelDistance;

    [SerializeField] private float rotationSpeed;

    [Space] [Header("Shooting")] [SerializeField]
    private float shootingDamage;

    [SerializeField] private float fireRate;
    [SerializeField] private Transform fireSource;
    [SerializeField] private LayerMask shootingLayerMasks;

    [Space] [Header("Other")] [SerializeField]
    private float onDroneHitStunnedSeconds;

    [SerializeField] private float idleTime;
    [SerializeField] private Transform modelTransform;
    [SerializeField] private GameObject droneModel;
    [SerializeField] private GameObject[] drops;

    [Space] [SerializeField] [ReadOnly] private DroneState currentState;

    private NavMeshAgent _agent;
    private Vector3 _currentDestination;
    private Transform _player;

    private StateMachine<DroneState> _stateMachine;

    private void Start()
    {
        ChangeState(DroneState.Idle);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    protected override void OnInit()
    {
        _agent = GetComponent<NavMeshAgent>();
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        _stateMachine = new StateMachine<DroneState>();

        _stateMachine.OnStatePhase(DroneState.Idle, StatePhase.Enter, () =>
        {
            StopMoving();
            Invoke(() => ChangeState(GetNextState()))
                .AfterSeconds(idleTime)
                .CancelIf(() => _stateMachine.CurrentState != DroneState.Idle);
        });

        _stateMachine.OnStatePhase(DroneState.Idle, StatePhase.Stay, () => OnStayState(null));

        _stateMachine.OnStatePhase(DroneState.Patrol, StatePhase.Enter, SetRandomDestination);

        _stateMachine.OnStatePhase(DroneState.Patrol, StatePhase.Stay, () =>
        {
            OnStayState(() =>
            {
                if (HasReachedDestination()) SetRandomDestination();
            });
        });

        _stateMachine.OnStatePhase(DroneState.Alert, StatePhase.Enter, StopMoving);

        _stateMachine.OnStatePhase(DroneState.Alert, StatePhase.Stay, () => OnStayState(RotateTowardsPlayer));

        _stateMachine.OnStatePhase(DroneState.Chase, StatePhase.Stay, () => OnStayState(FollowPlayer));

        _stateMachine.OnStatePhase(DroneState.Attack, StatePhase.Enter, () =>
        {
            _agent.SetDestination(transform.position);
            Invoke(ShootPlayer).EverySeconds(1 / fireRate).While(() => _stateMachine.CurrentState == DroneState.Attack);
        });

        _stateMachine.OnStatePhase(DroneState.Attack, StatePhase.Stay, () => OnStayState(RotateTowardsPlayer));

        _stateMachine.OnStatePhase(DroneState.Hit, StatePhase.Enter, () =>
        {
            _stateMachine.Lock();
            _agent.SetDestination(transform.position);
            Invoke(() =>
            {
                _stateMachine.Unlock();
                ChangeState(GetNextState());
            }).AfterSeconds(onDroneHitStunnedSeconds);
        });
    }

    private void OnStayState(Action stateAction)
    {
        FindPlayer();

        var next = GetNextState();

        if (next != _stateMachine.CurrentState)
        {
            ChangeState(next);
            return;
        }

        stateAction?.Invoke();
    }

    private void FindPlayer()
    {
        _player = detectionArea.VisibleTargets.FirstOrDefault();
    }

    protected override void OnTakeDamage()
    {
        transform.DOShakeRotation(onDroneHitStunnedSeconds, Vector3.one);
        if (currentState != DroneState.Hit) ChangeState(DroneState.Hit);
    }

    protected override void OnDie()
    {
        for (var i = 0; i < Random.Range(0, drops.Length); i++)
            Instantiate(drops[Random.Range(0, drops.Length)], modelTransform.position, Quaternion.identity);

        Instantiate(droneModel, modelTransform.position, modelTransform.rotation);
        Destroy(gameObject);
    }

    private void StopMoving()
    {
        _agent.SetDestination(transform.position);
    }

    private void FollowPlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void ShootPlayer()
    {
        if (!Physics.Raycast(fireSource.position, fireSource.forward, out var hit, Mathf.Infinity,
            shootingLayerMasks)) return;
        hit.collider.gameObject.GetComponent<IDamageTaker>()?.TakeDamage(shootingDamage);
        EventManager.TriggerEvent(EventData.instance.onShootHit, new Dictionary<string, object>
        {
            {"hit", hit}
        });
    }

    private void RotateTowardsPlayer()
    {
        var t = transform;
        var direction = (_player.position - t.position).normalized;
        direction.y = 0f;
        var rotation = Vector3.RotateTowards(
            t.forward,
            direction,
            rotationSpeed * Mathf.Deg2Rad * Time.deltaTime,
            0.0f);
        transform.rotation = Quaternion.LookRotation(rotation);
    }

    private void SetRandomDestination()
    {
        var direction = Random.insideUnitSphere * maxPatrolTravelDistance + transform.position;
        NavMesh.SamplePosition(direction, out var hit, maxPatrolTravelDistance, 1);
        _currentDestination = hit.position;
        _agent.SetDestination(_currentDestination);
    }

    private bool HasReachedDestination()
    {
        return Vector3.Distance(_currentDestination, transform.position) < 2f;
    }

    private DroneState GetNextState()
    {
        return true switch
        {
            true when IsPlayerInShootingRange() => DroneState.Attack,
            true when CanSeePlayer() => DroneState.Chase,
            true when IsPlayerInDetectionRange() => DroneState.Alert,
            _ => DroneState.Patrol
        };
    }

    private bool CanSeePlayer()
    {
        return sightArea.VisibleTargets.Count > 0;
    }

    private bool IsPlayerInDetectionRange()
    {
        return detectionArea.VisibleTargets.Count > 0;
    }

    private bool IsPlayerInShootingRange()
    {
        return shootingArea.VisibleTargets.Count > 0;
    }

    private void ChangeState(DroneState state)
    {
        _stateMachine.CurrentState = state;
        currentState = _stateMachine.CurrentState;
    }
}