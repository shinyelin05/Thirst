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
    public Entity enemyHP;
    bool isDieAnim = false;

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
        Debug.Log(state);
        enemySpeed = 2f;
        stateTimer = 0;
        curState = state;
    }

    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
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
                StartCoroutine(DieState());
                //  DieState();
                break;
        }
    }

    void IdleState()
    {

        Debug.Log("IDLE");

        if(stateTimer > 2)
        {
            ChangeState(EnemyState.Trance);
        }


    }

    void TranceState()
    {

        animator.SetBool("IsTrance", true);

        dist = Vector3.Distance(transform.position, player.gameObject.transform.position);

        if (stateTimer > 0)
        {
            stateTimer = -Random.Range(5f, 8f);
            idleMoveDir = Random.insideUnitSphere.normalized;

            idleMoveDir.y = 0;
        }

        transform.position += (Vector3)idleMoveDir * Time.deltaTime * enemySpeed;

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(idleMoveDir), Time.deltaTime * enemySpeed);

        if ( dist < 10)
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
        Debug.Log("Attack");
        animator.SetBool("IsAttack", true);

      if( stateTimer >= 5f)
        {
            animator.SetBool("IsAttack", false);
            ChangeState(EnemyState.Idle);
        }
    }

    IEnumerator DieState()
    {
        if (isDieAnim)
        {
            StopCoroutine(DieState());

        }

        animator.SetBool("IsRun", false);
        animator.SetBool("IsTrance", false);

        animator.SetTrigger("IsDie");

        yield return new WaitForSeconds(1.5f);
        Debug.Log("DIE");
        enemyHP.Die();


    }

    public void EnemyInit(PlayerController owner)
    {
        player = owner;
    }
}
