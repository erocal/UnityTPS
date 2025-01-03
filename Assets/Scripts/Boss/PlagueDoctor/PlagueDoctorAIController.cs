using UnityEngine;

public class PlagueDoctorAIController : AIController
{

    #region -- 參數參考區 --

    ActionSystem actionSystem;

    PlagueDoctorMover plagueDoctorMover;
    PlagueDoctorFighter plagueDoctorFighter;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        Init();

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

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化參數
    /// </summary>
    private void Init()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;

        organism = Organism.Instance;
        enemyRoot = this.gameObject;
        player = organism.GetPlayer();
        aiAnimator = this.GetComponent<Animator>();
        aiCollider = this.GetComponent<Collider>();
        health = this.GetComponent<Health>();

        plagueDoctorMover = GetComponent<PlagueDoctorMover>();
        plagueDoctorFighter = GetComponent<PlagueDoctorFighter>();

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
        if (IsRange() || CheckIsBeHit())
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

        plagueDoctorFighter.Attack(player.GetComponent<Health>());
    }

    // 困惑的動作行為
    protected override void ConfuseBehavior()
    {
        plagueDoctorMover.CancelMove();
        plagueDoctorFighter.CancelTarget();

        // 困惑動作
        aiAnimator.SetBool("IsConfuse", true);
    }

    // 巡邏行為
    protected override void PatrolBehavior()
    {
        Vector3 nextWaypointPostion = aiSpawnPostion;
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                plagueDoctorMover.CancelMove();
                aiAnimator.SetBool("IsConfuse", true);
                sinceArriveWayPointTimer = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumber(currentWaypointIndex);
            }

            if (sinceArriveWayPointTimer > waypointToWaitTime)
            {
                aiAnimator.SetBool("IsConfuse", false);
                plagueDoctorMover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex), patrolSpeedRatio);
            }
        }
        else
        {
            aiAnimator.SetBool("IsConfuse", false);
            plagueDoctorMover.MoveTo(aiSpawnPostion, 0.5f);
        }
    }

    /// <summary>
    /// 計時器
    /// </summary>
    private void UpdateTimer()
    {
        UpdateLastSawPlayerTimer();
        UpdateArriveWayPointTimer();
    }

    #endregion

    #region -- 事件相關 --

    /// <summary>
    /// PlagueDoctor死亡時處理方法
    /// </summary>
    protected override void OnDie(int id)
    {

        if (id != this.gameObject.GetInstanceID()) return;

        plagueDoctorMover.CancelMove();
        aiAnimator.SetTrigger("IsDead");
        aiCollider.enabled = false;
    }

    #endregion

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
