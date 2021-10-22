using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDoor : MonoBehaviour
{
    Animator animator;

    // Update is called once per frame
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    // void OnTriggerEnter(Collider other){
    //     if(other.gameObject.tag == "Player"){
    //         animator.Play("DoorAnimation");
    //         Debug.Log("EXECUTING");
    //     }
    // }

    // void OnTriggerExit(Collision collision){
    //     if(collision.gameObject.tag == "Player"){
    //         animator.Play("DoorAnimation_close");
    //     }
    // }

}
