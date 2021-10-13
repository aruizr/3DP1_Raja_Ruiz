using System;
using UnityEngine;

[Serializable]
public struct Vector3Flags
{
    public bool x, y, z;

    public bool AnyTrue()
    {
        return x || y || z;
    }
}

public class MovementLock : MonoBehaviour
{
    [SerializeField] private Vector3Flags lockPosition;
    [SerializeField] private Vector3Flags lockRotation;

    private (Vector3, Vector3) _values;

    private void Awake()
    {
        var t = transform;
        _values = (t.position, t.rotation.eulerAngles);
    }

    private void LateUpdate()
    {
        var t = transform;
        if (lockPosition.AnyTrue())
        {
            var originalPosition = _values.Item1;
            var transformPosition = t.position;
            t.position = new Vector3(
                lockPosition.x ? originalPosition.x : transformPosition.x,
                lockPosition.y ? originalPosition.y : transformPosition.y,
                lockPosition.z ? originalPosition.z : transformPosition.z);
        }

        if (lockRotation.AnyTrue())
        {
            var originalRotation = _values.Item2;
            var transformRotation = t.rotation.eulerAngles;
            t.rotation = Quaternion.Euler(new Vector3(
                lockRotation.x ? originalRotation.x : transformRotation.x,
                lockRotation.y ? originalRotation.y : transformRotation.y,
                lockRotation.z ? originalRotation.z : transformRotation.z));
        }
    }
}