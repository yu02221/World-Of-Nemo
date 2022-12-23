using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float liightSpeed;

    public GameObject Target;

    public float offsetX = 0.0f;
    public float offsetY = 0.0f; 
    public float offsetZ = 0.0f;

    public float SKYSpeed = 10.0f;
    Vector3 TargetPos;

    private void Update()
    {
        transform.Rotate(Vector3.right, (liightSpeed) * Time.deltaTime);
    }

    void FixedUpdate()
    {
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * SKYSpeed);
    }
}

