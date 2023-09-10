using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class EnemyController : Entity
{
    [Header("�� ������Ʈ ����")]
    EnemyState curState;
    float stateTimer;
    bool isDie;

    [Header("�� �̵� ����")]
    float enemySpeed = 2f;
    float enemyDist;
    Vector3 idleMoveDir;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    [Header("�� ü��")]
    public Entity enemyHP;
    public GameObject enemyHPSlider;
  //  public Volume volume; // Post-Processing Volume
    private Vignette vignette; // Vignette Effect

    [Header("�÷��̾� ����")]
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

    public override void Damage(float dmg)
    {
        Debug.Log("Damage");
        base.Damage(dmg);


        animator.SetBool("IsGetHit", true);
    }

    public void PlayerInit(PlayerController owner)
    {
        player = owner;
    }
}
