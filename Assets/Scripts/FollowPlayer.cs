using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    //����(2)-�̿�
    
    public float turnSpeed = 4.0f;
    private float xRotate = 0.0f;

    public GameObject player;

    void Update()
    {
        transform.position = player.transform.position;

        // �¿�� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� �¿�� ȸ���� �� ���
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        // ���� y�� ȸ������ ���� ���ο� ȸ������ ���
        float yRotate = transform.eulerAngles.y + yRotateSize;

        // ���Ʒ��� ������ ���콺�� �̵��� * �ӵ��� ���� ī�޶� ȸ���� �� ���(�ϴ�, �ٴ��� �ٶ󺸴� ����)
        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        // ���Ʒ� ȸ������ ���������� -45�� ~ 80���� ���� (-45:�ϴù���, 80:�ٴڹ���)
        // Clamp �� ���� ������ �����ϴ� �Լ�
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -80, 80);

        // ī�޶� ȸ������ ī�޶� �ݿ�(X, Y�ุ ȸ��)
        transform.eulerAngles = new Vector3(xRotate, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    

    //����(1)
    /*
    public GameObject player;

    bool zoomCheck = true;
    void Update()
    {
        if(zoomCheck)
            transform.position = player.transform.position; //��in
        else
        {
            transform.position = player.transform.position + new Vector3(0, 5.5f, -8);
        }

        if (Input.GetKeyDown("v"))
            zoomCheck = !zoomCheck;
    }
    */
}
