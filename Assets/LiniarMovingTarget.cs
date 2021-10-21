using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiniarMovingTarget : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    Vector3 startPosition;

    [SerializeField]
    Vector3 endPosition;

    [SerializeField]
    float speed;
    private float t;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime * speed;
        // Moves the object to target position
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
        // Flip the points once it has reached the target
        if (t >= 1)
        {
            var b = endPosition;
            var a = startPosition;
            startPosition = b;
            endPosition = a;
            t = 0;
        }
    }
}
