using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Jump,
    Crouch,
    Attack,
    Damaged,
    Dead,
}
public class PlayerMove : MonoBehaviour
{
    //���� �÷��̾��� ����
    public PlayerState playerState;

    //AnimationClip�� �̿��� ���¿� ���� ���� ��������
    //�������
    public Animator playerAnim;
    //�������

    //�÷��̾� ������ ���� ���� ����
    public float turnSpeed;
    public float moveSpeed;
    Rigidbody rb;
    //�� ������� �Ÿ� ����
    bool isBorder;
    //��

    //�÷��̾� ���� ���� ���� ����
    public Transform groundCheckTransform;
    public Vector3 boxSize = new Vector3(0f, 1f,01f);
    public float halfsize = 1;
    public LayerMask groundCheckLayerMask;
    public float jumpPower;
    bool isGround;
    //��

    Vector3 dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
            Jump();
    }

    private void FixedUpdate()
    {
        IsGroundCheck();
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
            Move();
        StopToWall();
    }

    void StopToWall()
    {
        dir = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (Input.GetKey(KeyCode.W))
        {
            isBorder = Physics.Raycast(dir, transform.forward, 0.55f, LayerMask.GetMask("Ground"));
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                isBorder = false;
        }
        else
            isBorder = false;
        
        Debug.DrawRay(dir, transform.forward * 0.55f, Color.red);
    }

    void Move()
    {
        //���콺 �����ӿ� ���� ���� �޴´�
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed * Time.fixedDeltaTime;
        //���� ���� �����Ѵ�
        float yRotate = transform.eulerAngles.y + yRotateSize;
        //���� ���콺 �������� ���ٸ� ���Ͱ��� ���� ���� 0���� ������Ų��.
        if (yRotateSize == 0)
            rb.angularVelocity = Vector3.zero;
        //�÷��̾��� ȸ���������� ������ ��������ش�
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        //playerState������ ���� �Է°��� ���� State������ �����Ѵ�
        //�÷��̾ ���� ���⿡ ���� �������� �Է°��� ������ش�.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerState = PlayerState.Run;
                playerAnim.SetBool("run", true);
                moveSpeed = 8;
            }
            else
            {
                playerState = PlayerState.Walk;
                playerAnim.SetBool("walk", true);
                playerAnim.SetBool("run", false);
                moveSpeed = 4;
            }
            
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            if(!isBorder) //���� ������ �Ÿ��� ���������� �۴ٸ� ����
            {
                transform.position += move * moveSpeed * Time.fixedDeltaTime;
            }
            playerState = PlayerState.Walk;
            playerAnim.SetBool("walk", true);
        }
        else if(isGround == true)
        {
            playerState = PlayerState.Idle;
            playerAnim.SetBool("walk", false);
            playerAnim.SetBool("run", false);

        }
    }

    //�÷��̾��� �ϴ�(�߹ٴ�)�ʿ� üũ�ڽ� ������Ʈ�� ��ġ���� Ground�� ���̾� �����̵� ������Ʈ�� �ݶ��̴��� ��ġ�� �Ǹ� ������ �����ϰԲ� boolüũ�� ���ش�
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
            playerState = PlayerState.Jump;
        }
    }
    void Jump()
    {
        if (playerState != PlayerState.Dead && playerState != PlayerState.Damaged &&
            playerState != PlayerState.Attack && playerState != PlayerState.Crouch &&
            isGround == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            else return;
        }
    }

    void Dead()
    {
        if (playerState == PlayerState.Dead)
            Time.timeScale = 0;
    }

    public void HitByEnemy(Vector3 enemyPosi0tion, float attackPower)
    {
        float hp = 0;
        Vector3 reactVec = transform.position - enemyPosi0tion;
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rb.AddForce(reactVec * 5, ForceMode.Impulse);
        hp -= attackPower;
    }

    public void Attack()
    {

    }
}
