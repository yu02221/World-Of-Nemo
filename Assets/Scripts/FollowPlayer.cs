using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float turnSpeed;
    private float xRotate = 0.0f;

    void Update()
    {
        // 좌우로 움직인 마우스의 이동량 * 속도에 따라 카메라가 좌우로 회전할 양 계산
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // 현재 y축 회전값에 더한 새로운 회전각도 계산
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // 위아래로 움직인 마우스의 이동량 * 속도에 따라 카메라가 회전할 양 계산(하늘, 바닥을 바라보는 동작)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // 위아래 회전량을 더해주지만 -45도 ~ 80도로 제한 (-45:하늘방향, 80:바닥방향)
        // Clamp 는 값의 범위를 제한하는 함수
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        // 카메라 회전량을 카메라에 반영(X, Y축만 회전)
        transform.eulerAngles = new Vector3(xRotate, transform.eulerAngles.y, 0);
    }
    

    //실험(1)
    /*
    public GameObject player;

    bool zoomCheck = true;
    void Update()
    {
        if (zoomCheck) //zoomCheck의 bool 타입이 참/거짓에 따라 시점, 조작모드 전환
            ZoomIn(); //1인칭뷰
        else
            ZoomOut(); //쿼터뷰

        if (Input.GetKeyDown("v"))
            zoomCheck = !zoomCheck;
    }

    //카메라의 기본 포지션은 잡았으나 추후 지형/플레이어 스케일 고려하여 수정예정
    void ZoomIn()
    {
        transform.position = player.transform.position; //줌in
    }

    void ZoomOut() 
    {
        transform.position = player.transform.position + new Vector3(0, 12.0f, -15); //줌 out
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }
    */
}
