using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsFollow : MonoBehaviour
{
    public GameObject FollowTarget;

    public float offsetX = -15f;
    public float offsetY = 180f;
    public float offsetZ = -15f;

    public float CloudsSpeed = 10.0f;

    Vector3 TargetPos;

    void FixedUpdate()
    {
        TargetPos = new Vector3(
            FollowTarget.transform.position.x + offsetX,
            FollowTarget.transform.position.y + offsetY,
            FollowTarget.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CloudsSpeed);
    }

}
