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
    //�÷��̾� �ɷ�ġ
    PlayerStatus ps;

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
    Vector3 velocity;
    float gravity = -10;
    //��

    TerrainChunk tc;

    Vector3 dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ps = GetComponent<PlayerStatus>();
    }
    private void Update()
    {
        IsGroundCheck();
        if (!isGround)
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = 0;
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
            Jump();

        //���콺 �����ӿ� ���� ���� �޴´�
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime;
        //���� ���� �����Ѵ�
        float yRotate = transform.eulerAngles.y + yRotateSize;
        //���� ���콺 �������� ���ٸ� ���Ͱ��� ���� ���� 0���� ������Ų��.
        if (yRotateSize == 0)
            rb.angularVelocity = Vector3.zero;
        //�÷��̾��� ȸ���������� ������ ��������ش�
        transform.eulerAngles = new Vector3(0, yRotate, 0);

        
    }

    private void FixedUpdate()
    {
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
            Move();

        transform.Translate(velocity * Time.fixedDeltaTime);
    }

    void Move()
    {
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
            if (MoveBlockCheck(move) == BlockType.Air || !isGround)
            {
                transform.position += move * moveSpeed * Time.fixedDeltaTime;
            }
            else
            {
                print(isGround);
                print(MoveBlockCheck(move));
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
        Collider[] cols = Physics.OverlapBox(groundCheckTransform.position, boxSize * 0.1f,
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
    private void Jump()
    {
        if (playerState != PlayerState.Dead && playerState != PlayerState.Damaged &&
            playerState != PlayerState.Attack && playerState != PlayerState.Crouch &&
            isGround == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpPower;
                //rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            else return;
        }
    }

    private BlockType MoveBlockCheck(Vector3 move)
    {
        Vector3 moveBlockPos = transform.position + move.normalized * 0.5f;
        int chunkPosX = Mathf.FloorToInt(moveBlockPos.x / 16f) * 16;
        int chunkPosZ = Mathf.FloorToInt(moveBlockPos.z / 16f) * 16;

        ChunkPos cp = new ChunkPos(chunkPosX, chunkPosZ);

        int bix = Mathf.FloorToInt(moveBlockPos.x) - chunkPosX;
        int biy = Mathf.FloorToInt(moveBlockPos.y);
        int biz = Mathf.FloorToInt(moveBlockPos.z) - chunkPosZ;

        tc = TerrainGenerator.buildedChunks[cp];
        return tc.blocks[bix, biy, biz];
    }

    void Dead()
    {
        if (playerState == PlayerState.Dead)
            Time.timeScale = 0;
    }

    public void HitByEnemy(Vector3 enemyPosi0tion, int attackPower)
    {
        Vector3 reactVec = transform.position - enemyPosi0tion;
        reactVec = reactVec.normalized;
        reactVec += Vector3.up;
        rb.AddForce(reactVec * 5, ForceMode.Impulse);
        ps.hp -= attackPower;
        print(ps.hp);
        if (ps.hp <= 0)
        {
            Dead();
            playerState = PlayerState.Dead;
        }
    }

    public void Attack()
    {

    }
}
