using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorTransform;

    private float _originalPositionY, _finalPositionY;

    [SerializeField]
    private float _offsetY;

    private void Awake()
    {
        _originalPositionY = doorTransform.position.y;
        _finalPositionY = _originalPositionY - _offsetY;
    }

    public void OpenDoor(){
        doorTransform.DOMoveY(_finalPositionY, 0.5f);
    }

    public void CloseDoor(){
        doorTransform.DOMoveY(_originalPositionY, 0.5f);
    }
}
