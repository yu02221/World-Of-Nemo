using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Text;

public class EnemyManager : MonoBehaviour
{
    public Transform groundCheckTransform; //에너미를 기준으로 Ground를 체크하기 위함
    public Vector3 boxSize = new Vector3(0, 1, 0); //그라운드 체크의 범위를 위한 벡터값
    public float halfsize = 1; //그라운드 체크의 범위를 줄이기 위한 변수
    public LayerMask groundCheckLayerMask; //그라운드의 레이어
    public float jumpPower; //점프할때의 힘
    bool isGround; // 만약 isGround라면 점프가 불가능, !isGround라면 점프가 가능.

    Vector3 dir;
    Vector3 lookDir;

    public Transform lookPlayer; //RotateTowards를 자연스럽게 해주기 위해 플레이어에게 시선을 고정시키는 자식obj
    
    public Transform player;
    public float distanceFromPlayer; //Enemy와 Player의 거리를 체크하기 위함
    public float speed; //이동할때 스피드
    public float turnSpeed; //턴(플레이어 방향으로)스피드

    Rigidbody rb;

    bool isBorder; //정면방향에 Ground를 체크해주기 위함

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lookDir = (player.position - transform.position).normalized;
        
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, Time.deltaTime);
        //점프가 가능한지 여부를 지속적으로 묻기 위한 메소드
        IsGroundCheck();
        //Enemy와 Player의 거리가 일정 수치 미만이 되었는지 체크하기위한 메소드
        CheckDistanceToPlayer();
        //만약 Enemy와 Player의 거리가 10미만이라면 플레이어 방향으로 이동
        if(distanceFromPlayer < 10)
        {
            
            //Enemy의 회전값을 부드럽게 변경해주기위함
            //transform.LookAt(player);
            //transform.rotation = Quaternion.RotateTowards(player.transform.rotation, transform.rotation, turnSpeed * 5);
            //transform.rotation = Quaternion.Euler(0, lookPlayer.rotation.x, 0);

            //Enemy의 이동을 담당
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            //움직일때만 메소드가 이용되게 하기 위해
            JumpToWall();
        }

        print("isGround" + isGround);
        print("isBorder" +isBorder);
    }

    //만약 벽 앞까지 온다면 점프
    void JumpToWall()
    {
        //벡터값 세팅
        dir = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
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
            isGround = true;
        else
            isGround = false;
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
    
    //FPS Game의 Enemy를 참고하여 만든 스크립트
    /*
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Die,
    }
    public EnemyState e_State;

    public Transform player;
    public float findDistance = 8f;
    public float moveSpeed = 5f;
    CharacterController cc;
    Vector3 originPos;
    Quaternion originRot;
    public float moveDistance = 20f;

    NavMeshAgent navmeshAgent;


    public int attackPower = 3;
    float currentTime = 0;
    float attackDelay = 2f;
    float attackDistance = 2f;
    void Start()
    {
        e_State = EnemyState.Idle;

        cc = GetComponent<CharacterController>();

        originPos = transform.position;
        originRot = transform.rotation;

        navmeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (e_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            e_State = EnemyState.Move;
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            e_State = EnemyState.Return;
        }
    }
    void Return()
    {
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            navmeshAgent.destination = originPos;
            navmeshAgent.stoppingDistance = 0;
        }
        else
        {
            navmeshAgent.isStopped = true;
            navmeshAgent.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;

            e_State = EnemyState.Idle;
        }
    }

    void Die()
    {
        StopAllCoroutines();

        Destroy(gameObject, 2f);
    }
    */
}
