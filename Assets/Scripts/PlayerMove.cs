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
    //ÇöÀç ÇÃ·¹ÀÌ¾îÀÇ »óÅÂ
    public PlayerState playerState;

    //AnimationClipÀ» ÀÌ¿ëÇØ »óÅÂ¿¡ ´ëÇÑ µ¿ÀÛ ±¸Çö¿¹Á¤
    //¿©±âºÎÅÍ

    //¿©±â±îÁö

    //ÇÃ·¹ÀÌ¾î ¿òÁ÷ÀÓ °ü·Ã º¯¼ö ½ÃÀÛ
    public float turnSpeed;
    public float moveSpeed;
    Rigidbody rb;
    //³¡

    //ÇÃ·¹ÀÌ¾î Á¡ÇÁ °ü·Ã º¯¼ö ½ÃÀÛ
    public Transform groundCheckTransform;
    public Vector3 boxSize = new Vector3(0f, 1f,01f);
    public float halfsize = 1;
    public LayerMask groundCheckLayerMask;
    public float jumpPower;
    bool isGround;
    
    //³¡

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        IsGroundCheck();
        if (playerState != PlayerState.Dead && playerState != PlayerState.Attack)
        {
            Move();
            Jump();
        }
    }
    void Move()
    {
        //¸¶¿ì½º ¿òÁ÷ÀÓ¿¡ ´ëÇÑ °ªÀ» ¹Þ´Â´Ù
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        //¹ÞÀº °ªÀ» ÀúÀåÇÑ´Ù
        float yRotate = transform.eulerAngles.y + yRotateSize;
        //¸¸¾à ¸¶¿ì½º ¿òÁ÷ÀÓÀÌ ¾ø´Ù¸é º¤ÅÍ°ª¿¡ ´ëÇÑ ÈûÀ» 0À¸·Î °íÁ¤½ÃÅ²´Ù.
        if (yRotateSize == 0)
            rb.angularVelocity = Vector3.zero;
        //ÇÃ·¹ÀÌ¾îÀÇ È¸Àü°ª¿¡´ëÇÑ ³»¿ëÀ» Àû¿ë½ÃÄÑÁØ´Ù
        transform.eulerAngles = new Vector3(0, yRotate, 0);
        //playerStateº¯°æÀ» À§ÇØ ÀÔ·Â°ª¿¡ ´ëÇÑ Stateº¯°æÀ» ÁøÇàÇÑ´Ù
        //ÇÃ·¹ÀÌ¾î°¡ º¸´Â ¹æÇâ¿¡ µû¸¥ ¿òÁ÷ÀÓÀÇ ÀÔ·Â°ªÀ» Ãâ·ÂÇØÁØ´Ù.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Vector3 move = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
            transform.position += move * moveSpeed * Time.deltaTime;
            playerState = PlayerState.Walk;
        }
        else if(isGround == true)
            playerState = PlayerState.Idle;
    }

    //ÇÃ·¹ÀÌ¾îÀÇ ÇÏ´Ü(¹ß¹Ù´Ú)ÂÊ¿¡ Ã¼Å©¹Ú½º ¿ÀºêÁ§Æ®¸¦ À§Ä¡½ÃÄÑ Ground·Î ·¹ÀÌ¾î ÁöÁ¤ÀÌµÈ ¿ÀºêÁ§Æ®¿Í ÄÝ¶óÀÌ´õ°¡ °ãÄ¡°Ô µÇ¸é Á¡ÇÁ°¡ °¡´ÉÇÏ°Ô²û boolÃ¼Å©¸¦ ÇØÁØ´Ù
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
        print(isGround);
        
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
}
