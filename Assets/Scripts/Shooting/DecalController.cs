using System.Collections.Generic;
using UnityEngine;

public class DecalController : ExtendedMonoBehaviour
{
    [SerializeField] private GameObject bulletDecalPrefab;
    [SerializeField] private int bulletDecalPoolSize;

    private IObjectPool<GameObject> _decalPool;

    private void Start()
    {
        _decalPool = new QueuePool<GameObject>(
            () =>
            {
                var decal = Instantiate(bulletDecalPrefab);
                decal.SetActive(false);
                return decal;
            },
            obj => obj.SetActive(true),
            null,
            bulletDecalPoolSize);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventData.Instance.onShootHit, OnShootHit);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventData.Instance.onShootHit, OnShootHit);
    }

    private void OnShootHit(Dictionary<string, object> message)
    {
        var hit = (RaycastHit) message["hit"];
        if (hit.transform.GetComponent<IDamageTaker>() != null) return;
        var decal = _decalPool.Get();
        decal.transform.parent = hit.transform;
        decal.transform.position = hit.point;
        decal.transform.rotation = Quaternion.LookRotation(hit.normal);
        _decalPool.Return(decal);
    }
}