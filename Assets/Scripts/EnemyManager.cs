using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Text;

public class EnemyManager : MonoBehaviour
{
    public enum E_State
    {
        Idle,
        Walk,
        Jump,
        Attack,
        Death,

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

    public int attackPower;
    public float attackDelay;
    public float waitForAttack;
    //public GameObject nearPlayer;
    
    PlayerMove pm;

    public Animator anim;

    public int maxHp;
    public int nowHp;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        pm = player.GetComponent<PlayerMove>();
        e_State = E_State.Idle;
    }

    private void Update()
    {
        //실험용
        Damaged();
        //Enemy와 Player의 거리가 일정 수치 미만이 되었는지 체크하기위한 메소드
        CheckDistanceToPlayer();
        //플레이어와의 거리가 1.3 미만일경우 공격
        if (distanceFromPlayer < 1.3f)
        {
            Attack();
            //anim.SetBool("attack", true);
            anim.SetBool("walk", false);
        }
        //플레이어와의 거리가 1.3초과 3 미만일경우 Idle상태로 변경
        else if (distanceFromPlayer > 1.3 && distanceFromPlayer < 3)
        { 
            e_State = E_State.Idle;
            //anim.SetBool("attack", false);
        }
        //플레이어와의 거리가 3초과일 경우 어택딜레이 초기화
        else if (distanceFromPlayer > 3)
            attackDelay = 0;

        //플레이어와의 거리를 체크하여 조건을 달성할시에 움직임 관련 메소드를 호출해주기 위함.
        if (distanceFromPlayer < 10 && e_State != E_State.Attack)
        {
            //Enemy 이동에 관한 메소드
            Move();
            anim.SetBool("walk", true);
            //벽 앞에서면 점프시켜주는 메소드
            JumpToWall();
            //점프가 가능한지 여부를 지속적으로 묻기 위한 메소드
            IsGroundCheck();
        }
        
        else if(e_State != E_State.Attack)
        { 
            e_State = E_State.Idle;
            anim.SetBool("walk", false);
        }
    }

    void Damaged()
    {//실험용 if문
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("damaged");
            nowHp -= 2;
        }
        Death();
    }
    //damaged를 어떻게 할지 생각(1회성으로)
    void Death()
    {
        if (nowHp <= 0)
        {
            anim.SetBool("death", true);
            e_State = E_State.Death;
            Destroy(gameObject, 3f);
        }
    }

    
    void Attack()
    {
        e_State = E_State.Attack;

        attackDelay += Time.deltaTime;

        if (attackDelay > 0.8)
        {
            anim.SetTrigger("attack");
            pm.HitByEnemy(transform.position, attackPower);
            attackDelay = 0;
        }
        
    }

    void Move()
    {
        //Enemy의 Rotation을 담당
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        if(e_State != E_State.Death && e_State != E_State.Attack)
            e_State = E_State.Walk;

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
        dir = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
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

            if (e_State != E_State.Death)
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
