using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Crouch,
        Attack,
        Damaged,
        Dead,
    }
public class PlayerMove : MonoBehaviour
{
    //���� �÷��̾��� ����
    public PlayerState playerState;

    //AnimationClip�� �̿��� ���¿� ���� ���� ��������
    //�������

    //�������

    //�÷��̾� ������ ���� ���� ����
    public float turnSpeed;
    public float moveSpeed;
    Rigidbody rb;
    //��

    //�÷��̾� ���� ���� ���� ����
    public Transform groundCheckTransform;
    public Vector3 boxSize = new Vector3(0f, 1f,01f);
    public float halfsize = 1;
    public LayerMask groundCheckLayerMask;
    public float jumpPower;
    bool isGround;
    
    //��

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        IsGroundCheck();
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
        {
            Move();
            Jump();
        }
    }
    void Move()
    {
        //���콺 �����ӿ� ���� ���� �޴´�
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        //���� ���� �����Ѵ�
        float yRotate = transform.eulerAngles.y + yRotateSize;
        //���� ���콺 �������� ���ٸ� ���Ͱ��� ���� ���� 0���� ������Ų��.
        if (yRotateSize == 0)
            rb.angularVelocity = Vector3.zero;
        //�÷��̾��� ȸ���������� ������ ��������ش�
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        //playerState������ ���� �Է°��� ���� State������ �����Ѵ�
        //�÷��̾ ���� ���⿡ ���� �������� �Է°��� ������ش�.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            transform.position += move * moveSpeed * Time.deltaTime;
            playerState = PlayerState.Walk;
        }
        else if(isGround == true)
            playerState = PlayerState.Idle;
    }

    void IsGroundCheck()
    {
        Collider[] cols = Physics.OverlapBox(groundCheckTransform.position, boxSize * 0.5f,
            groundCheckTransform.rotation,
            groundCheckLayerMask);
        if (cols.Length > 0)
        { 
            isGround = true;
        }
        else
        { 
            isGround = false;
            playerState = PlayerState.Jump;
        }
        print(isGround);
        
    }
    void Jump()
    {
        if (playerState != PlayerState.Dead && playerState != PlayerState.Damaged &&
            playerState != PlayerState.Attack && playerState != PlayerState.Crouch &&
            isGround == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            else return;
        }
    }
}
