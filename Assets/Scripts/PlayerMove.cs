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
    //현재 플레이어의 상태
    public PlayerState playerState;

    //AnimationClip을 이용해 상태에 대한 동작 구현예정
    //여기부터

    //여기까지

    //플레이어 움직임 관련 변수 시작
    public float turnSpeed;
    public float moveSpeed;
    Rigidbody rb;
    //끝

    //플레이어 점프 관련 변수 시작
    public Transform groundCheckTransform;
    public Vector3 boxSize = new Vector3(0f, 1f,01f);
    public float halfsize = 1;
    public LayerMask groundCheckLayerMask;
    public float jumpPower;
    bool isGround;
    
    //끝

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
        //마우스 움직임에 대한 값을 받는다
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        //받은 값을 저장한다
        float yRotate = transform.eulerAngles.y + yRotateSize;
        //만약 마우스 움직임이 없다면 벡터값에 대한 힘을 0으로 고정시킨다.
        if (yRotateSize == 0)
            rb.angularVelocity = Vector3.zero;
        //플레이어의 회전값에대한 내용을 적용시켜준다
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        //playerState변경을 위해 입력값에 대한 State변경을 진행한다
        //플레이어가 보는 방향에 따른 움직임의 입력값을 출력해준다.
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
