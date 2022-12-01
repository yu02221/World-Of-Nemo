using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    //실험(2)-미완
    public float turnSpeed;
    public float moveSpeed;
    public float jump;
    float xRotate = 0.0f;

    bool isJumping = false;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Move();
        Jump();
    }
    void Move()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        float xRotateSize = -Input.GetAxis("Mouse Y") * turnSpeed;
        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 80);

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        Vector3 move =
            transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal");

        transform.position += move * moveSpeed * Time.deltaTime;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isJumping == false)
            {
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                isJumping = true;
            }
            else return;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }

    //실험(1) (Zoom Out)
    /*
    float hAxis;
    float vAxis;

    public float jump;
    public float speed;
    public float rotSpeed;

    Rigidbody rb;

    Vector3 moveVec;

    //bool isGround = true;
    bool isJumping = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }
    private void FixedUpdate()
    {
        Move();
        Jump();
    }


    void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp
            (transform.rotation, Quaternion.LookRotation(moveVec), Time.deltaTime * rotSpeed);

        transform.LookAt(transform.position + moveVec);
    }

    void Jump() //둘 중 하나쓰자
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isJumping == false)
            {
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                isJumping = true;
            }
            else return;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    */

}
