using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DistanceDoor : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;

    private float _originalPositionY, _finalPositionY;

    private void Awake()
    {
        _originalPositionY = doorTransform.position.y;
        _finalPositionY = _originalPositionY - 3;
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player"))
        {
            doorTransform.DOMoveY(_finalPositionY, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            doorTransform.DOMoveY(_originalPositionY, 0.5f);
        }
    }

}
