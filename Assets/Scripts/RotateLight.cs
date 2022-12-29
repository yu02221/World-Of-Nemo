using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float rotSpeed;
    void Update()
    {
        transform.Rotate(Vector3.right, (rotSpeed) * Time.deltaTime);
    }
}
