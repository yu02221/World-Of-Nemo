using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushEnemy : MonoBehaviour
{
    public enum E_State
    {
        Idle,
        Walk,
        Jump,
        Die,
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

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
        pm = player.GetComponent<PlayerMove>();
        e_State = E_State.Idle;
    }

    private void Update()
    {
        //Enemy�� Player�� �Ÿ��� ���� ��ġ �̸��� �Ǿ����� üũ�ϱ����� �޼ҵ�
        CheckDistanceToPlayer();

        //�÷��̾���� �Ÿ��� üũ�Ͽ� ������ �޼��ҽÿ� ������ ���� �޼ҵ带 ȣ�����ֱ� ����.
        if (distanceFromPlayer < 10)
        {
            //Enemy �̵��� ���� �޼ҵ�
            Move();
            //�� �տ����� ���������ִ� �޼ҵ�
            JumpToWall();
            //������ �������� ���θ� ���������� ���� ���� �޼ҵ�
            IsGroundCheck();
        }

        else if (e_State != E_State.Die)
            e_State = E_State.Idle;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Player")
        {
            pm.HitByEnemy(collision.transform.position, 1);
        }
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

            if (e_State != E_State.Die)
                e_State = E_State.Jump;
        }

    }

    //���� �� �ձ��� �´ٸ� ����
    void JumpToWall()
    {
        //���Ͱ� ����
        dir = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        //���ʹ̰� ���� ���� ����������� bool Ÿ������ üũ
        isBorder = Physics.Raycast(dir, transform.forward, 1.0f, LayerMask.GetMask("Ground"));
        //���� ���ʹ̰� ���� ��������ٸ� (����ĳ��Ʈ) ����
        if (isBorder)
            Jump();
        Debug.DrawRay(dir, transform.forward * 1.0f, Color.red);
    }

    void Move()
    {
        //Enemy�� Rotation�� ���
        lookDir = (player.position - transform.position).normalized;
        lookDir.y = 0;
        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.Lerp(from, to, turnSpeed * Time.deltaTime);

        if (e_State != E_State.Die)
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

    //���� �޼ҵ�
    void Jump()
    {
        if (isGround == true)
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
}
