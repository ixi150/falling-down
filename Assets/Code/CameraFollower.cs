using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smothTime = 1;

    Vector3 velocity;

    void FixedUpdate()
    {
        if (target == null) return;

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smothTime);
    }
}
