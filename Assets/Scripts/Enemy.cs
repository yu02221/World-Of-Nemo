using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum E_State
    {
        Idle,
        Walk,
        Ready,
        Crush,
        Attack,
        Damaged,
        Death,
    }

    //�̵��� ���� ����
    public Transform player;
    public float distanceFromPlayer; //Enemy�� Player�� �Ÿ��� üũ�ϱ� ����
    public float speed; //�̵��Ҷ� ���ǵ�
    public float turnSpeed; //��(�÷��̾� ��������)���ǵ�
    public E_State e_State;

    public Rigidbody rb;
    public int maxHp;
    public int hp;

    public Animator anim;


    public float stateTime;

    //�÷��̾���� �Ÿ� ���
    public void CheckDistanceToPlayer()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceFromPlayer > 128)
            Destroy(gameObject);
    }

    public void HitByPlayer(Vector3 playerPosition, int damage)
    {
        if (e_State != E_State.Death)
        {
            Vector3 reactVec = transform.position - playerPosition;
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
            hp -= damage;
            e_State = E_State.Damaged;
            anim.SetTrigger("damaged");

            if (hp <= 0)
            {
                Death();
                e_State = E_State.Death;
            }
        }
    }

    public void Damaged()
    {
        stateTime += Time.deltaTime;
        if (hp > 0 && stateTime > 1)
        {
            print(hp);
            e_State = E_State.Idle;
            stateTime = 0;
        }
        if (hp <= 0)
        {
            Death();
        }
    }


    public void Death()
    {
        anim.SetBool("death", true);
        e_State = E_State.Death;
        player.GetComponent<PlayerStatus>().GetExp(10);
        Destroy(gameObject, 3f);
    }
}
