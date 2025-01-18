using UnityEngine;

public class ZombieAIController : AIController
{

    #region -- 資源參考區 --

    [Space(5)]
    [Header("閒置的音效")]
    [SerializeField] AudioClip zombieIdleSFX;
    [Header("追趕的音效")]
    [SerializeField] AudioClip zombieFollowSFX;

    #endregion

    #region -- 變數參考區 --

    Mover mover;
    Fighter fighter;

    bool isDead;

    // 計時器
    private float zombieFollowTimer = 2.0f;

    #endregion

    private void Awake()
    {

        Init();

        EnemySpawn();

        #region -- 委派 --

        SetAction();

        #endregion

    }

    private void Update()
    {

        if(isDead) return;

        AIBehavior();

        UpdateTimer();

    }

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化參數
    /// </summary>
    protected override void Init()
    {

        base.Init();

        mover = GetComponent<Mover>();
        fighter = GetComponent<Fighter>();

    }

    /// <summary>
    /// 設置委派事件
    /// </summary>
    private void SetAction()
    {

        actionSystem.OnDamage += OnDamage;
        actionSystem.OnDie += OnDie;

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
        aiAnimator.SetBool("IsConfuse", false);
        SawPlayer();
        if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombieFollowTimer <= 0)
        {
            ZombieFollow(gameObject);
            zombieFollowTimer = 2.0f;
        }
        fighter.Attack(player.GetComponent<Health>());
    }

    // 巡邏行為
    protected override void PatrolBehavior()
    {
        if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombieFollowTimer <= 0)
        {
            ZombieIdle(gameObject);
            zombieFollowTimer = 2.0f;
        }
        
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                aiAnimator.SetBool("IsConfuse", true);
                sinceArriveWayPointTimer = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumber(currentWaypointIndex);
            }

            if (sinceArriveWayPointTimer > waypointToWaitTime)
            {
                aiAnimator.SetBool("IsConfuse", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex), patrolSpeedRatio);
            }
        }
        else
        {
            aiAnimator.SetBool("IsConfuse", false);
            mover.MoveTo(aiSpawnPostion, 0.5f);
        }
    }

    // 困惑的動作行為
    protected override void ConfuseBehavior()
    {
        mover.CancelMove();
        fighter.CancelTarget();

        // 困惑動作
        aiAnimator.SetBool("IsConfuse", true);
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

    /// <summary>
    /// 播放Zombie追趕的音效
    /// </summary>
    /// <param name="zombie">傳入的物件，用來抓取聲音組件，此處應為Zombie</param>
    public void ZombieFollow(GameObject zombie)
    {

        aiAudioSource?.PlayOneShot(zombieFollowSFX);

    }

    /// <summary>
    /// 播放Zombie閒置的音效
    /// </summary>
    /// <param name="zombie">傳入的物件，用來抓取聲音組件，此處應為Zombie</param>
    public void ZombieIdle(GameObject zombie)
    {

        aiAudioSource?.PlayOneShot(zombieIdleSFX);

    }

    #region -- 事件相關 --

    /// <summary>
    /// AI死亡時處理方法
    /// </summary>
    protected override void OnDie(int id)
    {

        if (id != this.gameObject.GetInstanceID()) return;

        mover.CancelMove();
        aiAnimator.SetTrigger("IsDead");
        aiCollider.enabled = false;
        isDead = true;

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
