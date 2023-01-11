using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item item;
    public Material material;
    public int count;
    public Inventory hotInven;
    public Transform player;

    public float jump;

    public Rigidbody rb;

    [SerializeField]
    private float range = 0.5f;  // ������ ������ ������ �ִ� �Ÿ�

    [SerializeField]
    private LayerMask layerMask;  // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ������ �� �־�� �Ѵ�.

    float dis;
    float itemSpeed = 4f;

    private void Start()
    {
        GetComponent<MeshRenderer>().material = material;
    }

    private void Update()
    {
        Floating();
        CheckDistanc();
    }

    private void Floating() //���ִ°�
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, -transform.up, out hitInfo, range, layerMask))
        {
            rb.AddForce(transform.up * jump, ForceMode.Impulse);
        }
    }
    private void CheckDistanc() // �����Ÿ� ���� ���
    {
        dis = Vector3.Distance(player.position, transform.position);
        //float distance = Vector3.Distance(player.position, transform.position);
        if (dis <= 3)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, itemSpeed * Time.deltaTime);
        }
        if (dis <= 0.5)
        {
            hotInven.AddItem(item, count);
            Destroy(gameObject);
        }

    }
}