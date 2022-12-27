using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : MonoBehaviour
{
    public enum E_State
    {
        Idle,
        Walk,
        Ready,
        Crush,
        Damaged,
        Death,
    }
    public E_State e_State;

    //�̵��� ���� ����
    public GameObject nearPlayer;
    public Transform player;
    public float distanceFromPlayer; //Enemy�� Player�� �Ÿ��� üũ�ϱ� ����
    public float speed; //�̵��Ҷ� ���ǵ�
    public float turnSpeed; //��(�÷��̾� ��������)���ǵ�

    //�̵����� �� ��(�׶���)üũ�� ���� ������
    public Transform groundCheckTransform; //���ʹ̸� �������� Ground�� üũ�ϱ� ����
    public Vector3 boxSize = new Vector3(0, 1, 0); //�׶��� üũ�� ������ ���� ���Ͱ�
    public LayerMask groundCheckLayerMask; //�׶����� ���̾�
    public float halfsize = 1; //�׶��� üũ�� ������ ���̱� ���� ����
    public float jumpPower; //�����Ҷ��� ��
    bool isGround; // ���� isGround��� ������ �Ұ���, !isGround��� ������ ����.
    bool isBorder; //������⿡ Ground�� üũ���ֱ� ����
    bool isForwardToWall; //��(Ground)üũ

    Vector3 dir;
    Vector3 lookDir;
    Rigidbody rb;

    //�����Ҷ� ����ϴ� ������
    public int crushPower = 2;
    float crushTime;
    PlayerMove pm;
    Vector3 playerTransSave;
    float readyTime;

    //������� �׼��� ���� ����
    float stateTime;

    //ũ���� ���ʹ� stats
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
        CheckDistanceToPlayer(); //�÷��̾���� �Ÿ� üũ
        IsGroundCheck(); //boolŸ���� �̿��� ���� üũ
        JumpToWall(); //���⼭ ����üũ �� ���� ����
        if (Input.GetKeyDown(KeyCode.K))
        {
            e_State = E_State.Damaged;
            anim.SetTrigger("damaged");
            nowHp -= 2;
        }

        switch (e_State)
        {
            case E_State.Idle:
                Idle();
                break;
            case E_State.Walk:
                Walk();
                break;
            case E_State.Ready:
                Ready();
                break;
            case E_State.Crush:
                Crush();
                break;
            case E_State.Damaged:
                Damaged();
                break;
            case E_State.Death:
                Death();
                break;
        }
    }
    //�Ÿ��� 20 �̸� Walk
    //�Ÿ��� 10 �̸� Ready > Crush
    void Idle()
    {
        if (distanceFromPlayer < 20)
            e_State = E_State.Walk;
        else if (distanceFromPlayer > 20)
            e_State = E_State.Idle;
    }

    void Walk()
    {
        anim.SetBool("walk", true);
        //Enemy�� Rotation�� ���
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        //Ground�� �������� �浹�� ���� �� �����ӿ� ������ �α�����
        dir = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isForwardToWall = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        if (isForwardToWall)
            speed = 0;
        else
            speed = 2;
        //Enemy�� �̵��� ���
        transform.position = Vector3.MoveTowards
            (transform.position, player.transform.position, speed * Time.deltaTime);

        if(distanceFromPlayer < 7)
        {
            e_State = E_State.Ready;
        }
    }
    
    void Ready()
    {
        //Enemy�� Rotation�� ���
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        anim.SetBool("walk", false);
        readyTime += Time.deltaTime;
        playerTransSave = player.transform.position;
        anim.SetBool("ready", true);
        if (readyTime > 1.3f)
        { 
            Crush();
            print("Ready > Crush");
            anim.SetBool("ready", false);
            readyTime = 0;
        }
        
    }

    void Crush()
    {
        crushTime += Time.deltaTime;

        if (crushTime < 2)
        {
            speed = 5;
            e_State = E_State.Crush;
            transform.position =
                Vector3.MoveTowards(transform.position, playerTransSave, speed * Time.deltaTime);
            anim.SetBool("crush", true);
        }
        if ( crushTime > 2)
        {
            e_State = E_State.Idle;
            print("Crush > Idle");
            anim.SetBool("crush", false);
            crushTime = 0;
            speed = 2;
        }
        if ( distanceFromPlayer > 20 )
        { 
            e_State = E_State.Idle;
            speed = 2;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Player")
        {
            pm.HitByEnemy(collision.transform.position, 1);
        }
    }

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
        Destroy(gameObject, 4f);
    }

    //�÷��̾���� �Ÿ� ���
    void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }

    //���� ���� ���θ� Ȯ���ϱ����� �޼ҵ�
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

    //���� �� �ձ��� �´ٸ� ����
    void JumpToWall()
    {
        //���Ͱ� ����
        dir = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //���ʹ̰� ���� ���� ����������� bool Ÿ������ üũ
        isBorder = Physics.Raycast(dir, transform.forward, 1.8f, LayerMask.GetMask("Ground"));
        //���� ���ʹ̰� ���� ��������ٸ� (����ĳ��Ʈ) ����
        if (isBorder)
            Jump();
        Debug.DrawRay(dir, transform.forward * 1.8f, Color.red);
    }
    void Jump()
    {
        if (isGround == true)
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}
