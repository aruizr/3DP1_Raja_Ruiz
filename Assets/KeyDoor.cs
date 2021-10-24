using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : Door
{
    [SerializeField]
    string keyID;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player"))
        {
            KeyStore ks = other.gameObject.GetComponent<KeyStore>();
            if(ks.DoPlayerHaveKey(keyID)){
                OpenDoor();
            }
        }
    }
}
