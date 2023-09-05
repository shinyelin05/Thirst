using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.InspectorCurveEditor;

public class EnemyController : MonoBehaviour
{
    [Header("적 스테이트 관련")]
    EnemyState curState;
    float stateTimer;

    Vector3 idleMoveDir;
    public PlayerController player;
    public Animator animator;
    float enemySpeed = 2f;
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

    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
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
        enemySpeed = 2f;

        animator.SetBool("IsTrance", true);

        dist = Vector3.Distance(transform.position, player.gameObject.transform.position);

        if (stateTimer > 0)
        {
            stateTimer = -Random.Range(5f, 8f);
            idleMoveDir = Random.insideUnitSphere.normalized;

            idleMoveDir.y = 0;
        }

        transform.position += (Vector3)idleMoveDir * Time.deltaTime * enemySpeed;

        if (dist < 20)
        {
            animator.SetBool("IsTrance", false);
            ChangeState(EnemyState.Chase);
        }
    }

    void ChaseState()
    {
        enemySpeed = 4f;

        animator.SetBool("IsRun", true);

        float dist = Vector3.Distance(transform.position, player.gameObject.transform.position);
        Vector3 dir = (player.gameObject.transform.position - transform.position).normalized;
        
        dir.y = 0;
        
        transform.position += dir * enemySpeed * Time.deltaTime;

        Vector3 dir2 = player.transform.position - this.transform.position;

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir2), Time.deltaTime * enemySpeed);


        // 만약에 거리가 5보다 적으면 Idle로 하기.
        if (dist > 20)
        {
            animator.SetBool("IsRun", false);
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
