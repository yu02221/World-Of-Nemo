using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushEnemy : MonoBehaviour
{
    public enum E_State
    {
        Idle,
        Walk,
        Ready,
        Crush,
        Jump,
        Damaged,
        Death,
    }
    public E_State e_State;

    //이동을 위한 변수
    public GameObject nearPlayer;
    public Transform player;
    public float distanceFromPlayer; //Enemy와 Player의 거리를 체크하기 위함
    public float speed; //이동할때 스피드
    public float turnSpeed; //턴(플레이어 방향으로)스피드

    //이동관련 중 벽(그라운드)체크를 위한 변수들
    public Transform groundCheckTransform; //에너미를 기준으로 Ground를 체크하기 위함
    public Vector3 boxSize = new Vector3(0, 1, 0); //그라운드 체크의 범위를 위한 벡터값
    public LayerMask groundCheckLayerMask; //그라운드의 레이어
    public float halfsize = 1; //그라운드 체크의 범위를 줄이기 위한 변수
    public float jumpPower; //점프할때의 힘
    bool isGround; // 만약 isGround라면 점프가 불가능, !isGround라면 점프가 가능.
    bool isBorder; //정면방향에 Ground를 체크해주기 위함
    bool isForwardToWall; //벽(Ground)체크

    Vector3 dir;
    Vector3 lookDir;
    Rigidbody rb;

    //공격할때 사용하는 변수들
    public int crushPower = 2;
    PlayerMove pm;
    Vector3 playerTransSave;
    float readyTime;

    //대미지드 액션을 위한 변수
    float stateTime;

    //크러쉬 에너미 stats
    public int maxHp;
    public int nowHp;

    public Animator anim;

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
        if (e_State == E_State.Damaged)
        { 
            stateTime += Time.deltaTime;
            if (stateTime > 1)
            { 
                e_State = E_State.Idle;
                stateTime = 0;
            }
        }

        //Enemy와 Player의 거리가 일정 수치 미만이 되었는지 체크하기위한 메소드
        CheckDistanceToPlayer();

            //벽 앞에서면 점프시켜주는 메소드
            JumpToWall();
            
            //점프가 가능한지 여부를 지속적으로 묻기 위한 메소드
            IsGroundCheck();

        //플레이어와의 거리를 체크하여 조건을 달성할시에 움직임 관련 메소드를 호출해주기 위함.
        if (distanceFromPlayer < 20 && distanceFromPlayer > 10 && //플레이어와 거리가 10초과 20미만일때
            e_State != E_State.Death && e_State != E_State.Ready && 
            e_State != E_State.Damaged && e_State != E_State.Crush)
        {
            anim.SetBool("walk", true);
            
            //Enemy 이동 제어에 관한 메소드
            if (distanceFromPlayer > 1.0f) 
            { 
            Move();
            }
            
        }

        else if (distanceFromPlayer < 10)//플레이어와의 거리가 10미만일때
        {
            Ready();
        }

        else if (distanceFromPlayer > 10 && e_State != E_State.Death)
        { 
            e_State = E_State.Idle;
            anim.SetBool("walk", false);
        }
    }

    void Ready()
    {
        readyTime += Time.deltaTime;
        e_State = E_State.Ready;
        playerTransSave = player.transform.position;
        anim.SetBool("walk", false);
        anim.SetTrigger("ready");

        if (readyTime > 1.5f)
        { 
            Crush();
            readyTime = 0;
        }
    }

    void Crush()
    {
        e_State = E_State.Crush;

        anim.SetTrigger("crush");

        speed = 10;
        
        if (distanceFromPlayer < 10)
            transform.position =
            Vector3.MoveTowards(transform.position, playerTransSave, speed * Time.deltaTime);

        if (transform.position == playerTransSave)
        { 
            e_State = E_State.Idle;
            anim.SetTrigger("idle");
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Player")
        {
            pm.HitByEnemy(collision.transform.position, 1);
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

        if (e_State != E_State.Death)
            e_State = E_State.Walk;

        //Ground와 지속적인 충돌을 감지 및 움직임에 제한을 두기위함
        dir = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isForwardToWall = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        if (isForwardToWall)
            speed = 0;
        else
            speed = 5;
        //Enemy의 이동을 담당
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

    }


    //플레이어까지의 거리를 계산
    void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
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
    //만약 벽 앞까지 온다면 점프
    void JumpToWall()
    {
        //벡터값 세팅
        dir = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //에너미가 전방 벽과 가까워졌는지 bool 타입으로 체크
        isBorder = Physics.Raycast(dir, transform.forward, 1.8f, LayerMask.GetMask("Ground"));
        //만약 에너미가 벽과 가까워졌다면 (레이캐스트) 점프
        if (isBorder)
            Jump();
        Debug.DrawRay(dir, transform.forward * 1.8f, Color.red);
    }
    void Damaged()
    {//실험용 if문
        //stateTime += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.K))
        { 
        anim.SetTrigger("damaged");
        nowHp -= 2;
        e_State = E_State.Damaged;
        }
        Death();
    }

    void Death()
    {
        if (nowHp <= 0)
        {
            anim.SetBool("death", true);
            e_State = E_State.Death;
            Destroy(gameObject,3f);
        }
    }


}
