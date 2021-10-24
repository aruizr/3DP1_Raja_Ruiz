using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDoor : Door
{

    [SerializeField]
    string eventName;

     void OnEnable(){
        EventManager.StartListening(eventName, EventExecuted);
    }

    void OnDisable(){
        EventManager.StopListening(eventName, EventExecuted);
    }

    
    void EventExecuted(Dictionary<string,object> args){
        OpenDoor();
    }

}
