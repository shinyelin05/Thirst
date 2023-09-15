using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyController : Entity
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

    RaycastHit hit;
    float maxDistance = 15f;


    [Header("적 체력")]
    public Entity enemyHP;
    public GameObject enemyHPSlider;
    public Volume volume;
    bool isDamaged = false;

    [Header("플레이어 참조")]
    static PlayerController player;

    enum EnemyState
    {
        Idle,
        Trance,
        Chase,
        Attack,
        Hit,
        Die,
    }

    void ChangeState(EnemyState state)
    {
        Debug.Log("Change :" + state);
        enemySpeed = 2f;
        stateTimer = 0;
        curState = state;
    }

    public override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.blue, 0.3f);
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
            {
                if (hit.collider == null)
                {
                    return;
                }
                if (hit.collider.CompareTag("STONE"))
                {
                    Debug.Log(hit.collider.name + "돌!!!!!!!!!!!!!!!!!!!!!!!");

                }
            }

        }

        if (enemyHP.Hp <= 0)
        {

            curState = EnemyState.Die;
        }

        if (isDamaged)
        {
            ChangeState(EnemyState.Hit);
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

            case EnemyState.Hit:
                HitState();
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

        navMeshAgent.SetDestination(player.transform.position);

        this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
            Quaternion.LookRotation(dir), Time.deltaTime * enemySpeed);

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
        player.PlayerDamage(0.1f);
        player.isDamage = true;

        if (stateTimer >= 2f)
        {
            ChangeState(EnemyState.Idle);
            animator.SetBool("IsAttack", false);
            player.isDamage = false;
        }
    }

    void DieState()
    {
        animator.SetBool("IsRun", false);
        animator.SetBool("IsTrance", false);

        Destroy(enemyHPSlider);

        if (!isDie)
        {
            animator.SetTrigger("IsDie");
            isDie = true;
        }
    }

    void HitState()
    {

        animator.SetBool("IsRun", false);
        animator.SetBool("IsGetHit", true);

        if (stateTimer > 1)
        {
            ChangeState(EnemyState.Idle);
            animator.SetBool("IsGetHit", false);
        }
    }

    public override void Damage(float dmg)
    {
        Debug.Log("Damage");
        base.Damage(dmg);
        isDamaged = true;

        ChangeState(EnemyState.Hit);
        volume.weight = 0.0f;
    }

    public void PlayerInit(PlayerController owner)
    {
        player = owner;

    }
}
