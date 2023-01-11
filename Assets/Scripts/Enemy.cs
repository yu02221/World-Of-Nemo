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

    //이동을 위한 변수
    public Transform player;
    public float distanceFromPlayer; //Enemy와 Player의 거리를 체크하기 위함
    public float speed; //이동할때 스피드
    public float turnSpeed; //턴(플레이어 방향으로)스피드
    public E_State e_State;

    public Rigidbody rb;
    public int maxHp;
    public int hp;

    public Animator anim;


    public float stateTime;

    //플레이어까지 거리 계산
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
