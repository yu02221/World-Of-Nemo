using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Text;

public class EnemyManager : MonoBehaviour
{
    

    //FPS Game의 Enemy를 참고하여 만든 스크립트
    /*
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Die,
    }
    public EnemyState e_State;

    public Transform player;
    public float findDistance = 8f;
    public float moveSpeed = 5f;
    CharacterController cc;
    Vector3 originPos;
    Quaternion originRot;
    public float moveDistance = 20f;

    NavMeshAgent navmeshAgent;


    public int attackPower = 3;
    float currentTime = 0;
    float attackDelay = 2f;
    float attackDistance = 2f;
    void Start()
    {
        e_State = EnemyState.Idle;

        cc = GetComponent<CharacterController>();

        originPos = transform.position;
        originRot = transform.rotation;

        navmeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        switch (e_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            e_State = EnemyState.Move;
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            e_State = EnemyState.Return;
        }
    }
    void Return()
    {
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            navmeshAgent.destination = originPos;
            navmeshAgent.stoppingDistance = 0;
        }
        else
        {
            navmeshAgent.isStopped = true;
            navmeshAgent.ResetPath();

            transform.position = originPos;
            transform.rotation = originRot;

            e_State = EnemyState.Idle;
        }
    }

    void Die()
    {
        StopAllCoroutines();

        Destroy(gameObject, 2f);
    }
    */
}
