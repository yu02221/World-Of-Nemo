using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class CrushEnemy : MonoBehaviour
{
    public enum E_State
    {
        Idle,
        Walk,
        Jump,
        Attack,
        Die,
    }
    public E_State e_State;

    public Transform groundCheckTransform; //에너미를 기준으로 Ground를 체크하기 위함
    public Vector3 boxSize = new Vector3(0, 1, 0); //그라운드 체크의 범위를 위한 벡터값
    public float halfsize = 1; //그라운드 체크의 범위를 줄이기 위한 변수
    public LayerMask groundCheckLayerMask; //그라운드의 레이어
    public float jumpPower; //점프할때의 힘
    bool isGround; // 만약 isGround라면 점프가 불가능, !isGround라면 점프가 가능.

    Vector3 dir;
    Vector3 lookDir;

    public Transform player;
    public float distanceFromPlayer; //Enemy와 Player의 거리를 체크하기 위함
    public float speed; //이동할때 스피드
    public float turnSpeed; //턴(플레이어 방향으로)스피드

    Rigidbody rb;

    bool isBorder; //정면방향에 Ground를 체크해주기 위함
    bool isForwardToWall;

    public GameObject nearPlayer;

    PlayerManager pm;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerManager>();
        e_State = E_State.Idle;

    }

    private void Update()
    {
        //Enemy와 Player의 거리가 일정 수치 미만이 되었는지 체크하기위한 메소드
        CheckDistanceToPlayer();

        if (distanceFromPlayer <= 1.2)
            StartCoroutine(Attack());
        
        //플레이어와의 거리를 체크하여 조건을 달성할시에 움직임 관련 메소드를 호출해주기 위함.
        if (distanceFromPlayer < 10 && e_State != E_State.Attack)
        {
            //Enemy 이동에 관한 메소드
            Move();
            //벽 앞에서면 점프시켜주는 메소드
            JumpToWall();
            //점프가 가능한지 여부를 지속적으로 묻기 위한 메소드
            IsGroundCheck();
        }

        else if (e_State != E_State.Attack && e_State != E_State.Die)
            e_State = E_State.Idle;
    }

    IEnumerator Attack()
    {
        e_State = E_State.Attack;

        yield return new WaitForSeconds(1.5f);

        e_State = E_State.Idle;
    }

    void Move()
    {
        if (e_State != E_State.Die && e_State != E_State.Attack)
            e_State = E_State.Walk;

        //Enemy의 Rotation을 담당
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        //Ground와 지속적인 충돌을 감지 및 움직임에 제한을 두기위함
        dir = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isForwardToWall = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        if (isForwardToWall)
            speed = 0;
        else
            speed = 2;
        //Enemy의 이동을 담당
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

    }

    //만약 벽 앞까지 온다면 점프
    void JumpToWall()
    {
        //벡터값 세팅
        dir = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //에너미가 전방 벽과 가까워졌는지 bool 타입으로 체크
        isBorder = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        //만약 에너미가 벽과 가까워졌다면 (레이캐스트) 점프
        if (isBorder)
            Jump();
        Debug.DrawRay(dir, transform.forward * 1.0f, Color.red);
    }

    //점프 가능 여부를 확인하기위한 메소드
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

            if (e_State != E_State.Die)
                e_State = E_State.Jump;
        }

    }

    //점프 메소드
    void Jump()
    {
        if (isGround == true)
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

    //플레이어까지의 거리를 계산
    void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

}
