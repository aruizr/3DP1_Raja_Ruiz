using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStore : MonoBehaviour
{
    List<string> keys;

    void OnEnable(){
        
        EventManager.StartListening("key_collected", KeyCollectedEventListener);
        
    }

    void OnDisable(){
        EventManager.StopListening("key_collected", KeyCollectedEventListener);
        
    }

    void KeyCollectedEventListener(Dictionary<string,object> args){
        if(args["key"] != null){
            Debug.Log(args["key"] + " COLLECTED!");
            keys.Add((string)args["key"]);
        }
    }

    public bool DoPlayerHaveKey(string key){
        if(keys.Contains(key)){
            keys.Remove(key);
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
