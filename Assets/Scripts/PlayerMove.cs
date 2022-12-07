using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //실험(2)-미완
    
    public float turnSpeed = 4.0f;
    public float moveSpeed = 4.0f;
    float xRotate = 0.0f;

    private void Update()
    {
        Move();
    }
    void Move()
    {
        float yRotateSize = Input.GetAxis("Mouse X") * turnSpeed;
        float yRotate = transform.eulerAngles.y + yRotateSize;

        transform.eulerAngles = new Vector3(xRotate, yRotate, 0);

        Vector3 move =
            transform.forward * Input.GetAxis("Vertical") +
            transform.right * Input.GetAxis("Horizontal");

        transform.position += move * moveSpeed * Time.deltaTime;
    }
    

    //실험(1)
    /*
    float hAxis;
    float vAxis;
    public float jump;
    public float speed;
    public float rotSpeed;
    Rigidbody body;
    Vector3 moveVec;
    bool isGround = true;
    

    private void Start()
    {
        //body = GetComponent<Rigidbody>(); 
    }
    private void FixedUpdate()
    {
        //실험(1)
        //Move();
        //Jump();

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
    
    void Jump()
    {
        
        if(Input.GetKey(KeyCode.Space) && isGround)
        {
            body.AddForce(Vector3.up * jump, ForceMode.Impulse);

            isGround = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    
    */
}
