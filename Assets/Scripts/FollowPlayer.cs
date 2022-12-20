using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float turnSpeed;
    private float xRotate = 0.0f;

    void Update()
    {
        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed * Time.deltaTime;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);
        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(xRotate, transform.eulerAngles.y, 0);
    }
    

    //����(1)
    /*
    public GameObject player;

    bool zoomCheck = true;
    void Update()
    {
        if (zoomCheck) //zoomCheck�� bool Ÿ���� ��/������ ���� ����, ���۸�� ��ȯ
            ZoomIn(); //1��Ī��
        else
            ZoomOut(); //���ͺ�

        if (Input.GetKeyDown("v"))
            zoomCheck = !zoomCheck;
    }

    //ī�޶��� �⺻ �������� ������� ���� ����/�÷��̾� ������ ����Ͽ� ��������
    void ZoomIn()
    {
        transform.position = player.transform.position; //��in
    }

    void ZoomOut() 
    {
        transform.position = player.transform.position + new Vector3(0, 12.0f, -15); //�� out
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }
    */
}
