using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    string keyID;

    //OnTriggerEnter executar event de recollida de clau.

    // void OnTriggerEnter(Collider other){
    //     if(other.gameObject.tag == "Player"){
    //         EventManager.TriggerEvent("key_collected", new Dictionary<string, object>(){
    //             {
    //             "key" , keyID
    //             },
    //         });

    //         gameObject.SetActive(false);
    //     }
    // }

    public override void Interact(){
        EventManager.TriggerEvent("key_collected", new Dictionary<string, object>(){
                {
                "key" , keyID
                },
        });

        gameObject.SetActive(false);
    }
}
