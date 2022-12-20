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
    public Animator playerAnim;
    //여기까지

    //플레이어 움직임 관련 변수 시작
    public float turnSpeed;
    public float moveSpeed;
    Rigidbody rb;
    //블럭 측면과의 거리 측정
    bool isBorder;
    //끝

    //플레이어 점프 관련 변수 시작
    public Transform groundCheckTransform;
    public Vector3 boxSize = new Vector3(0f, 1f,01f);
    public float halfsize = 1;
    public LayerMask groundCheckLayerMask;
    public float jumpPower;
    bool isGround;
    //끝

    Vector3 dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
            Jump();
    }

    private void FixedUpdate()
    {
        IsGroundCheck();
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
            Move();
        StopToWall();
    }

    void StopToWall()
    {
        dir = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (Input.GetKey(KeyCode.W))
        {
            isBorder = Physics.Raycast(dir, transform.forward, 0.55f, LayerMask.GetMask("Ground"));
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                isBorder = false;
        }
        else
            isBorder = false;
        
        Debug.DrawRay(dir, transform.forward * 0.55f, Color.red);
    }

    void Move()
    {
        //마우스 움직임에 대한 값을 받는다
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed * Time.fixedDeltaTime;
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerState = PlayerState.Run;
                playerAnim.SetBool("run", true);
                moveSpeed = 8;
            }
            else
            {
                playerState = PlayerState.Walk;
                playerAnim.SetBool("walk", true);
                playerAnim.SetBool("run", false);
                moveSpeed = 4;
            }
            
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            if(!isBorder) //만약 벽과의 거리가 설정값보다 작다면 움직
            {
                transform.position += move * moveSpeed * Time.fixedDeltaTime;
            }
            playerState = PlayerState.Walk;
            playerAnim.SetBool("walk", true);
        }
        else if(isGround == true)
        {
            playerState = PlayerState.Idle;
            playerAnim.SetBool("walk", false);
            playerAnim.SetBool("run", false);

        }
    }

    //플레이어의 하단(발바닥)쪽에 체크박스 오브젝트를 위치시켜 Ground로 레이어 지정이된 오브젝트와 콜라이더가 겹치게 되면 점프가 가능하게끔 bool체크를 해준다
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

    void Dead()
    {
        if (playerState == PlayerState.Dead)
            Time.timeScale = 0;
    }

    public void HitByEnemy(Vector3 enemyPosi0tion, float attackPower)
    {
        float hp = 0;
        Vector3 reactVec = transform.position - enemyPosi0tion;
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rb.AddForce(reactVec * 5, ForceMode.Impulse);
        hp -= attackPower;
    }

    public void Attack()
    {

    }
}
