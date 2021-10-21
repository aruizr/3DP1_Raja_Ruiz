using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGalleryController : MonoBehaviour
{

    void OnEnable(){
        EventManager.StartListening("monitor_destroyed", MonitorDestroyer);
    }

    void OnDisable(){
        EventManager.StopListening("monitor_destroyed", MonitorDestroyer);
    }

    void MonitorDestroyer(Dictionary<string,object> args){
    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
