using UnityEngine;

public class BulletImpactFXController : MonoBehaviour
{
    [SerializeField] private GameObject impactFXPrefab;
    [SerializeField] private int impactFXPoolSize;

    private IObjectPool<GameObject> _impactFXPool;

    private void Start()
    {
        _impactFXPool = new QueuePool<GameObject>(
            () =>
            {
                var impactFX = Instantiate(impactFXPrefab, transform);
                impactFX.GetComponent<ParticleSystem>().Stop();
                return impactFX;
            },
            obj =>
            {
                var ps = obj.GetComponent<ParticleSystem>();
                ps.Clear();
                ps.Play();
            },
            null,
            impactFXPoolSize);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.instance.onShootHitEventName, OnShootHitEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.instance.onShootHitEventName, OnShootHitEvent);
    }

    private void OnShootHitEvent(object[] args)
    {
        var hit = (RaycastHit) args[0];
        var impactFX = _impactFXPool.Get();
        impactFX.transform.position = hit.point;
        impactFX.transform.rotation = Quaternion.LookRotation(hit.normal);
        _impactFXPool.Return(impactFX);
    }
}