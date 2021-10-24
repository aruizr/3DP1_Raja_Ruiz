using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DistanceDoor : Door
{


    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            CloseDoor();
            
        }
    }

}
