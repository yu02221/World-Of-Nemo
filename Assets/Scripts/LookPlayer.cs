using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    public Transform player;
    public float turnSpeed;
    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(player);
        //transform.rotation = Quaternion.Euler(0, player.position.y, 0);
        //transform.eulerAngles = new Vector3(0, player.transform.eulerAngles.y, 0);
    }
}
