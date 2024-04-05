using UnityEngine;

public class ZombieAIController : AIController
{

    #region -- 變數參考區 --

    Organism organism;

    Mover mover;
    Fighter fighter;
    ZombieAudio zombieAudio;

    // 計時器
    private float zombieFollowTimer = 2.0f;

    #endregion

    private void Awake()
    {
        organism = Organism.Instance;

        player = organism.GetPlayer();
        mover = GetComponent<Mover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<Fighter>();
        collider = GetComponent<Collider>();

        SetSpawnPosition(transform.position);

        #region -- 委派 --

        SetAction();

        #endregion
    }

    private void Update()
    {
        AIBehavior();

        UpdateTimer();
    }

    #region -- 方法參考區 --

    /// <summary>
    /// 設置委派事件
    /// </summary>
    private void SetAction()
    {
        health.onDamage += OnDamage;
        health.onDie += OnDie;
    }

    /// <summary>
    /// AI所能做出的行為
    /// </summary>
    protected override void AIBehavior()
    {
        // 玩家在追趕範圍內
        if (IsRange() || CheckIsBeHit() || fighter.actorType == Actor.Zombie)
        {
            AttackBehavior();
        }
        else if (sinceLastSawPlayerTimer < confuseTime)
        {
            ConfuseBehavior();
        }
        else
        {
            PatrolBehavior();
        }
    }

    protected override void AttackBehavior()
    {
        animator.SetBool("IsConfuse", false);
        SawPlayer();
        if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombieFollowTimer <= 0)
        {
            zombieAudio = GetComponent<ZombieAudio>();
            zombieAudio.ZombieFollow(gameObject);
            zombieFollowTimer = 2.0f;
        }
        fighter.Attack(player.GetComponent<Health>());
    }

    // 巡邏行為
    protected override void PatrolBehavior()
    {
        if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombieFollowTimer <= 0)
        {
            zombieAudio = GetComponent<ZombieAudio>();
            zombieAudio.ZombieIdle(gameObject);
            zombieFollowTimer = 2.0f;
        }
        Vector3 nextWaypointPostion = aiSpawnPostion;
        
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                animator.SetBool("IsConfuse", true);
                sinceArriveWayPointTimer = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumber(currentWaypointIndex);
            }

            if (sinceArriveWayPointTimer > waypointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex), patrolSpeedRatio);
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(aiSpawnPostion, 0.5f);
        }
    }

    // 困惑的動作行為
    protected override void ConfuseBehavior()
    {
        mover.CancelMove();
        fighter.CancelTarget();

        // 困惑動作
        animator.SetBool("IsConfuse", true);
    }

    /// <summary>
    /// 計時器
    /// </summary>
    private void UpdateTimer()
    {
        UpdateZombieFollowTimer();
        UpdateLastSawPlayerTimer();
        UpdateArriveWayPointTimer();
    }

    /// <summary>
    /// 更新殭屍追趕聲音計時器
    /// </summary>
    private void UpdateZombieFollowTimer()
    {
        zombieFollowTimer -= Time.deltaTime;
    }

    #region -- 事件相關 --

    /// <summary>
    /// AI死亡時處理方法
    /// </summary>
    protected override void OnDie()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
        collider.enabled = false;
    }

    #endregion

    #region -- DrawGizmo --

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    #endregion

    #endregion
}
