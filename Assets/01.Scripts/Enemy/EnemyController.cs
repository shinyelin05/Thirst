using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class EnemyController : MonoBehaviour
{
    [Header("적 스테이트 관련")]
    EnemyState curState;
    float stateTimer;

    Vector2 idleMoveDir;
    public PlayerController player;

    float enemySpeed = 3f;
    float dist;

    enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Die,
    }

    void ChangeState(EnemyState state)
    {
        Debug.Log(state);
        stateTimer = 0;
        curState = state;
    }

    private void Update()
    {
         

        stateTimer += Time.deltaTime;
        switch (curState)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Chase:
                ChaseState();
                break;

            case EnemyState.Attack:
                AttackState();
                break;

            case EnemyState.Die:
                DieState();
                break;
        }
    }

    void IdleState()
    {
        dist = Vector3.Distance(transform.position, player.gameObject.transform.position);

        if (stateTimer > 0)
        {
            stateTimer = -Random.Range(2.5f, 5f);
            idleMoveDir = Random.insideUnitCircle.normalized;
        }

        transform.position += (Vector3)idleMoveDir * Time.deltaTime * enemySpeed;

        if (dist < 10)
        {
            ChangeState(EnemyState.Chase);
        }
    }

    void ChaseState()
    {
        float dist = Vector3.Distance(transform.position, player.gameObject.transform.position);
        Vector3 dir = (player.gameObject.transform.position - transform.position).normalized;
        transform.position += dir * enemySpeed * Time.deltaTime;

        // 만약에 거리가 5보다 적으면 Idle로 하기.
        if (dist > 10)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    void AttackState()
    {
        //  
    }

    void DieState()
    {
        //
    }

    public void EnemyInit(PlayerController owner)
    {
        player = owner;
    }
}
