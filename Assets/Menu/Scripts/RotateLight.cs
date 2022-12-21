using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float liightSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.right, (liightSpeed) * Time.deltaTime);
    }
}
