using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableMonitor : HealthSystem
{
    [SerializeField]
    private GameObject screenExplosionParticleSystem;
    [SerializeField]
    private GameObject screenOff;
    [SerializeField]
    private GameObject screenOn;
    [SerializeField]
    private GameObject shards;
    public bool broken;
    private Transform initialPosition;

    // Start is called before the first frame update
    void Start()
    {

        initialPosition = gameObject.transform;

        EventManager.TriggerEvent("subscribe_monitor", new Dictionary<string, object>(){
                {
                "monitor" , gameObject
                },
            });
    }

    protected override void OnDie(){
        if ((!broken))
        {
            broken = true;
            screenOff.SetActive(false);
            screenOn.SetActive(false);
            shards.SetActive(true);
            Rigidbody[] shardRBs = GetComponentsInChildren<Rigidbody>();
            screenExplosionParticleSystem.SetActive(true);
            foreach (Rigidbody shardRB in shardRBs){
                float randomForce = Random.Range(1,5);
                float randomRotationX = Random.Range(-20,20);
                float randomRotationY = Random.Range(-20,20);
                float randomRotationZ = Random.Range(-20,20);
                shardRB.transform.Rotate(randomRotationX,randomRotationY,randomRotationZ);
                shardRB.AddRelativeForce(Vector3.forward * randomForce,ForceMode.Impulse);
            }

            EventManager.TriggerEvent("monitor_destroyed", new Dictionary<string, object>(){
                {
                "monitor" , gameObject
                },
            });
        }

    }

    public void resetMonitor(){
        
        if ((broken))
        {
            broken = false;
            screenOn.SetActive(true);
            shards.SetActive(false);
            
            screenExplosionParticleSystem.SetActive(false);
            
        }

    }
}
