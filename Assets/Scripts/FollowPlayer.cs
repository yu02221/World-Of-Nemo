using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    //실험(2)-미완
    
    public float turnSpeed = 4.0f;
    private float xRotate = 0.0f;

    public GameObject player;

    void Update()
    {
        transform.position = player.transform.position;

        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -80, 80);

        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(xRotate, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    

    //실험(1)
    /*
    public GameObject player;

    bool zoomCheck = true;
    void Update()
    {
        if(zoomCheck)
            transform.position = player.transform.position; //줌in
        else
        {
            transform.position = player.transform.position + new Vector3(0, 5.5f, -8);
        }

        if (Input.GetKeyDown("v"))
            zoomCheck = !zoomCheck;
    }
    */
}
