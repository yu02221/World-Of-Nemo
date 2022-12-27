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
        //�����
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

        //Enemy�� Player�� �Ÿ��� ���� ��ġ �̸��� �Ǿ����� üũ�ϱ����� �޼ҵ�
        CheckDistanceToPlayer();

            //�� �տ����� ���������ִ� �޼ҵ�
            JumpToWall();
            
            //������ �������� ���θ� ���������� ���� ���� �޼ҵ�
            IsGroundCheck();

        //�÷��̾���� �Ÿ��� üũ�Ͽ� ������ �޼��ҽÿ� ������ ���� �޼ҵ带 ȣ�����ֱ� ����.
        if (distanceFromPlayer < 20 && distanceFromPlayer > 10 && //�÷��̾�� �Ÿ��� 10�ʰ� 20�̸��϶�
            e_State != E_State.Death && e_State != E_State.Ready && 
            e_State != E_State.Damaged && e_State != E_State.Crush)
        {
            anim.SetBool("walk", true);
            
            //Enemy �̵� ��� ���� �޼ҵ�
            if (distanceFromPlayer > 1.0f) 
            { 
            Move();
            }
            
        }

        else if (distanceFromPlayer < 10)//�÷��̾���� �Ÿ��� 10�̸��϶�
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
        //Enemy�� Rotation�� ���
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        if (e_State != E_State.Death)
            e_State = E_State.Walk;

        //Ground�� �������� �浹�� ���� �� �����ӿ� ������ �α�����
        dir = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isForwardToWall = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        if (isForwardToWall)
            speed = 0;
        else
            speed = 5;
        //Enemy�� �̵��� ���
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

    }


    //�÷��̾������ �Ÿ��� ���
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

            if (e_State != E_State.Death)
                e_State = E_State.Jump;
        }

    }

    //���� �޼ҵ�
    void Jump()
    {
        if (isGround == true)
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
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
    void Damaged()
    {//����� if��
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
