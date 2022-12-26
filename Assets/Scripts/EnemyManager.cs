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

    public Transform groundCheckTransform; //���ʹ̸� �������� Ground�� üũ�ϱ� ����
    public Vector3 boxSize = new Vector3(0, 1, 0); //�׶��� üũ�� ������ ���� ���Ͱ�
    public float halfsize = 1; //�׶��� üũ�� ������ ���̱� ���� ����
    public LayerMask groundCheckLayerMask; //�׶����� ���̾�
    public float jumpPower; //�����Ҷ��� ��
    bool isGround; // ���� isGround��� ������ �Ұ���, !isGround��� ������ ����.

    Vector3 dir;
    Vector3 lookDir;

    public Transform player;
    public float distanceFromPlayer; //Enemy�� Player�� �Ÿ��� üũ�ϱ� ����
    public float speed; //�̵��Ҷ� ���ǵ�
    public float turnSpeed; //��(�÷��̾� ��������)���ǵ�

    Rigidbody rb;

    bool isBorder; //������⿡ Ground�� üũ���ֱ� ����
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
        //�����
        Damaged();
        //Enemy�� Player�� �Ÿ��� ���� ��ġ �̸��� �Ǿ����� üũ�ϱ����� �޼ҵ�
        CheckDistanceToPlayer();
        //�÷��̾���� �Ÿ��� 1.3 �̸��ϰ�� ����
        if (distanceFromPlayer < 1.3f)
        {
            Attack();
            //anim.SetBool("attack", true);
            anim.SetBool("walk", false);
        }
        //�÷��̾���� �Ÿ��� 1.3�ʰ� 3 �̸��ϰ�� Idle���·� ����
        else if (distanceFromPlayer > 1.3 && distanceFromPlayer < 3)
        { 
            e_State = E_State.Idle;
            //anim.SetBool("attack", false);
        }
        //�÷��̾���� �Ÿ��� 3�ʰ��� ��� ���õ����� �ʱ�ȭ
        else if (distanceFromPlayer > 3)
            attackDelay = 0;

        //�÷��̾���� �Ÿ��� üũ�Ͽ� ������ �޼��ҽÿ� ������ ���� �޼ҵ带 ȣ�����ֱ� ����.
        if (distanceFromPlayer < 10 && e_State != E_State.Attack)
        {
            //Enemy �̵��� ���� �޼ҵ�
            Move();
            anim.SetBool("walk", true);
            //�� �տ����� ���������ִ� �޼ҵ�
            JumpToWall();
            //������ �������� ���θ� ���������� ���� ���� �޼ҵ�
            IsGroundCheck();
        }
        
        else if(e_State != E_State.Attack)
        { 
            e_State = E_State.Idle;
            anim.SetBool("walk", false);
        }
    }

    void Damaged()
    {//����� if��
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("damaged");
            nowHp -= 2;
        }
        Death();
    }
    //damaged�� ��� ���� ����(1ȸ������)
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
        //Enemy�� Rotation�� ���
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        if(e_State != E_State.Death && e_State != E_State.Attack)
            e_State = E_State.Walk;

        //Ground�� �������� �浹�� ���� �� �����ӿ� ������ �α�����
        dir = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        isForwardToWall = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        if (isForwardToWall)
            speed = 0;
        else
            speed = 2;
        //Enemy�� �̵��� ���
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        

    }
    
    //���� �� �ձ��� �´ٸ� ����
    void JumpToWall()
    {
        //���Ͱ� ����
        dir = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
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

    //�÷��̾������ �Ÿ��� ���
    void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
    }
}
