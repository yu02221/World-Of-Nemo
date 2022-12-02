using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            Vector3 move =
                transform.forward * Input.GetAxis("Vertical") +
                transform.right * Input.GetAxis("Horizontal");

            transform.position += move * moveSpeed * Time.deltaTime;
        }
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
}
