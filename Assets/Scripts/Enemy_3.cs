using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemy
{
    /*
    public enum E_State
    {
        Idle,
        Walk,
        Damaged,
        Attack,
        Death,

    }
    public E_State e_State;
    */
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

    //Rigidbody rb;

    bool isBorder; //정면방향에 Ground를 체크해주기 위함
    bool isForwardToWall;
    /*
    public Animator anim;

    public int maxHp;
    public int nowHp;

    float stateTime;
    */
    int attackPower;
    float attackDelay;
    public GameObject bullet;
    public Transform firePos;

    private float burnTime;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        e_State = E_State.Idle;
    }

    private void Update()
    {
        CheckDistanceToPlayer(); //플레이어와의 거리 체크
        IsGroundCheck(); //bool타입을 이용한 점프 체크
        JumpToWall(); //여기서 점프체크 및 점프 실행
        if (Input.GetKeyDown(KeyCode.K))
        {
            e_State = E_State.Damaged;
            anim.SetTrigger("damaged");
            hp -= 2;
        }

        switch (e_State)
        {
            case E_State.Idle:
                Idle();
                break;
            case E_State.Walk:
                Walk();
                break;
            case E_State.Damaged:
                Damaged();
                break;
            case E_State.Attack:
                Attack();
                break;
            case E_State.Death:
                Death();
                break;
        }
        if (DayAndNight.tState == DayAndNight.TimeState.Day)
        {
            burnTime += Time.deltaTime;
            if (burnTime > 2f)
            {
                burnTime = 0;
                HitByPlayer(lookDir, 3);
            }
        }
    }
    //거리가 15이상 idle
    //거리가 15미만 10이상 walk
    //거리가 10미만 attack
    void Idle()
    {
        if ( distanceFromPlayer > 15 && e_State != E_State.Death )
        {
            e_State = E_State.Idle;
            anim.SetBool("walk", false);
        }
        if ( distanceFromPlayer < 15 && distanceFromPlayer > 10)
        {
            e_State = E_State.Walk;
        }
        if (distanceFromPlayer < 10)
        {
            e_State = E_State.Attack;
            anim.SetBool("walk", false);
        }
    }

    void Walk()
    {
        anim.SetBool("walk", true);
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
            speed = 3;
        //Enemy의 이동을 담당
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if (distanceFromPlayer < 10)
        {
            e_State = E_State.Attack;
            anim.SetBool("walk", false);
        }
    }

    void Attack()
    {
        //Enemy의 Rotation을 담당
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);


        attackDelay += Time.deltaTime;
        stateTime += Time.deltaTime;
        e_State = E_State.Attack;

        if (attackDelay > 3)
        {
            anim.SetTrigger("attack");
            attackDelay = 0;
            stateTime = 0;
            Instantiate(bullet, firePos.transform.position, firePos.transform.rotation);
        }

        if ( stateTime > 2.9f && distanceFromPlayer > 10)
        {
            attackDelay = 0;
            stateTime = 0;
            e_State = E_State.Idle;
        }
        else if ( stateTime > 3)
        {
            stateTime = 0;
        }
        
    }
    /*
    void Damaged()
    {
        stateTime += Time.deltaTime;
        if (nowHp > 0 && stateTime > 1)
        {
            e_State = E_State.Idle;
            stateTime = 0;
        }
        if (nowHp <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        anim.SetBool("death", true);
        e_State = E_State.Death;
        Destroy(gameObject, 3f);
    }
    */
    //플레이어까지 거리 계산
    void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer > 128)
            Destroy(gameObject);
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
        }

    }

    //만약 벽 앞까지 온다면 점프
    void JumpToWall()
    {
        //벡터값 세팅
        dir = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
        //에너미가 전방 벽과 가까워졌는지 bool 타입으로 체크
        isBorder = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        //만약 에너미가 벽과 가까워졌다면 (레이캐스트) 점프
        if (isBorder)
            Jump();
        Debug.DrawRay(dir, transform.forward * 1.0f, Color.red);
    }
    void Jump()
    {
        if (isGround == true)
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }

}
