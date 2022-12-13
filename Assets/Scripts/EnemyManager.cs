using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Text;

public class EnemyManager : MonoBehaviour
{
    public Transform groundCheckTransform; //���ʹ̸� �������� Ground�� üũ�ϱ� ����
    public Vector3 boxSize = new Vector3(0, 1, 0); //�׶��� üũ�� ������ ���� ���Ͱ�
    public float halfsize = 1; //�׶��� üũ�� ������ ���̱� ���� ����
    public LayerMask groundCheckLayerMask; //�׶����� ���̾�
    public float jumpPower; //�����Ҷ��� ��
    bool isGround; // ���� isGround��� ������ �Ұ���, !isGround��� ������ ����.

    Vector3 dir;
    Vector3 lookDir;

    public Transform lookPlayer; //RotateTowards�� �ڿ������� ���ֱ� ���� �÷��̾�� �ü��� ������Ű�� �ڽ�obj
    
    public Transform player;
    public float distanceFromPlayer; //Enemy�� Player�� �Ÿ��� üũ�ϱ� ����
    public float speed; //�̵��Ҷ� ���ǵ�
    public float turnSpeed; //��(�÷��̾� ��������)���ǵ�

    Rigidbody rb;

    bool isBorder; //������⿡ Ground�� üũ���ֱ� ����

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
        //������ �������� ���θ� ���������� ���� ���� �޼ҵ�
        IsGroundCheck();
        //Enemy�� Player�� �Ÿ��� ���� ��ġ �̸��� �Ǿ����� üũ�ϱ����� �޼ҵ�
        CheckDistanceToPlayer();
        //���� Enemy�� Player�� �Ÿ��� 10�̸��̶�� �÷��̾� �������� �̵�
        if(distanceFromPlayer < 10)
        {
            
            //Enemy�� ȸ������ �ε巴�� �������ֱ�����
            //transform.LookAt(player);
            //transform.rotation = Quaternion.RotateTowards(player.transform.rotation, transform.rotation, turnSpeed * 5);
            //transform.rotation = Quaternion.Euler(0, lookPlayer.rotation.x, 0);

            //Enemy�� �̵��� ���
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            //�����϶��� �޼ҵ尡 �̿�ǰ� �ϱ� ����
            JumpToWall();
        }

        print("isGround" + isGround);
        print("isBorder" +isBorder);
    }

    //���� �� �ձ��� �´ٸ� ����
    void JumpToWall()
    {
        //���Ͱ� ����
        dir = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        //���ʹ̰� ���� ���� ����������� bool Ÿ������ üũ
        isBorder = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        //���� ���ʹ̰� ���� ��������ٸ� (����ĳ��Ʈ) ����
        if (isBorder)
            Jump();
        Debug.DrawRay(dir, transform.forward * 1.0f, Color.red);
    }

    //���� ���� ���θ� Ȯ���ϱ����� �޼ҵ�
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

    //���� �޼ҵ�
    void Jump()
    {
        if (isGround == true)
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }



    //�÷��̾������ �Ÿ��� ���
    void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }
    
    //FPS Game�� Enemy�� �����Ͽ� ���� ��ũ��Ʈ
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
