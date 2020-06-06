using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    void Update()
    {
        this.transform.position = new Vector3(target.position.x +offset.x, this.transform.position.y + offset.y,
            target.position.z + offset.z);
    }
}
