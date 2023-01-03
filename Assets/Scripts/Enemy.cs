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
    public E_State e_State;

    public Rigidbody rb;
    public int maxHp;
    public int hp;

    public Animator anim;

    public float stateTime;

    public void HitByPlayer(Vector3 playerPosition, int damage)
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
        Destroy(gameObject, 3f);
    }
}
