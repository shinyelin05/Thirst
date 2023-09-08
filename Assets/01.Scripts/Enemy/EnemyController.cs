using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("적 스테이트 관련")]
    EnemyState curState;
    float stateTimer;
    bool isDie;

    [Header("적 이동 관련")]
    float enemySpeed = 2f;
    float enemyDist;
    Vector3 idleMoveDir;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    [Header("적 체력")]
    public Entity enemyHP;

    [Header("플레이어 참조")]
    static PlayerController player;


    enum EnemyState
    {
        Idle,
        Trance,
        Chase,
        Attack,
        Die,
    }

    void ChangeState(EnemyState state)
    {
        // Debug.Log(state);
        enemySpeed = 2f;
        stateTimer = 0;
        curState = state;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (enemyHP.Hp <= 0)
        {
            curState = EnemyState.Die;
        }

        stateTimer += Time.deltaTime;
        switch (curState)
        {
            case EnemyState.Idle:
                IdleState();
                break;

            case EnemyState.Trance:
                TranceState();
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
        if (stateTimer > 1)
        {
            ChangeState(EnemyState.Trance);
        }
    }

    void TranceState()
    {
        animator.SetBool("IsTrance", true);

        enemyDist = Vector3.Distance(transform.position, player.gameObject.transform.position);

        if (stateTimer > 0)
        {
            stateTimer = -Random.Range(5f, 8f);
            idleMoveDir = Random.insideUnitSphere.normalized;

            idleMoveDir.y = 0;
        }


        navMeshAgent.SetDestination(transform.position += (Vector3)idleMoveDir * Time.deltaTime * enemySpeed);

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(idleMoveDir), Time.deltaTime * enemySpeed);

        if (enemyDist < 10)
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

        navMeshAgent.SetDestination(transform.position += dir * enemySpeed * Time.deltaTime);

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * enemySpeed);

        if (dist >= 10)
        {
            animator.SetBool("IsRun", false);
            ChangeState(EnemyState.Trance);
        }
        else if (dist < 1)
        {
            animator.SetBool("IsRun", false);
            ChangeState(EnemyState.Attack);
        }
    }

    void AttackState()
    {
        animator.SetBool("IsAttack", true);
        player.PlayerDamage(10f);
      //  player.isDamage = true;

         if (stateTimer >= 2f)
        {
            ChangeState(EnemyState.Idle);
            animator.SetBool("IsAttack", false);
          //  player.isDamage = false;

        }
    }

    void DieState()
    {
        animator.SetBool("IsRun", false);
        animator.SetBool("IsTrance", false);
        if (!isDie)
        {
            animator.SetTrigger("IsDie");
            isDie = true;
        }
    }

    public void PlayerInit(PlayerController owner)
    {
        player = owner;
    }
}
