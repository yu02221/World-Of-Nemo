using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Text;

public class EnemyManager : MonoBehaviour
{
    public Transform groundCheckTransform;
    public Vector3 boxSize = new Vector3(0, 1, 0);
    public float halfsize = 1;
    public LayerMask groundCheckLayerMask;
    public float jumpPower;
    bool isGround;

    Vector3 dir;
    Vector3 playerDir;
    
    public Transform player;
    public float distanceFromPlayer;
    public float speed;
    public float turnSpeed;

    Rigidbody rb;

    bool isBorder;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        IsGroundCheck();
        playerDir = new Vector3(player.position.x, 0, player.position.z);

        print("isGround" + isGround);
        print("isBorder" +isBorder);

        CheckDistanceToPlayer();
        if(distanceFromPlayer < 10)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, playerDir, turnSpeed);
            //transform.position -= player.transform.forward * speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            //움직일때만 메소드가 이용되게 하기 위해
            JumpToWall();
        }
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
