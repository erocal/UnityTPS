using UnityEngine;

public class RootAIController : MonoBehaviour
{
    #region -- 物件參考區 --

    [Tooltip("追趕距離")]
    [SerializeField] float chaseDistance = 10f;
    [Tooltip("失去目標後困惑的時間")]
    [SerializeField] float confuseTime = 5f;

    [Header("巡邏路徑")]
    [SerializeField] PatrolPath patrol;
    [Header("需要到達WayPoint的距離")]
    [SerializeField] float waypointToStay = 3f;
    [Header("待在WayPoint的時間")]
    [SerializeField] float waypointToWaitTime = 3f;
    [Header("巡邏時的速度")]
    [Range(0, 1)]
    [SerializeField] float patrolSpeedRatio = 0.5f;

    [Space(20)]
    [Header("要銷毀的gameobject根節點")]
    [SerializeField] GameObject enemyRoot;

    #endregion

    #region -- 變數參考區 --

    GameObject player;
    RootMover mover;
    Animator animator;
    Health health;
    RootFighter fighter;
    Collider collider;
    MutantAudio mutantAudio;

    // 上次看到玩家的時間
    private float timeSinceLastSawPlayer = Mathf.Infinity;
    // 原點座標
    private Vector3 beginPostion;
    // 當前需要到達的WayPoint編號
    int currentWaypointIndex = 0;
    // 距離上次抵達WayPoint的時間
    float timeSinceArriveWayPoint = 0;

    // 計時器
    private float roarrate = 2.0f;

    bool isBeHit;

    #endregion

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mover = GetComponent<RootMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<RootFighter>();
        collider = GetComponent<Collider>();
        mutantAudio = GetComponent<MutantAudio>();

        beginPostion = transform.position;

        #region -- 訂閱 --

        health.onDamage += OnDamage;
        health.onDie += OnDie;

        #endregion
    }

    private void Update()
    {
        if (health.IsDead()) return;

        // 玩家在追趕範圍內
        if (IsRange() || isBeHit)
        {
            AttackBehavior();
        }
        else if (timeSinceLastSawPlayer < confuseTime)
        {
            ConfuseBehavior();
        }
        else
        {
            PatrolBehavior();
        }

        UpdateTimer();
    }

    #region -- 方法參考區 --

    /// <summary>
    /// 攻擊行為
    /// </summary>
    private void AttackBehavior()
    {
        animator.SetBool("IsConfuse", false);
        timeSinceLastSawPlayer = 0;
        fighter.Attack(player.GetComponent<Health>());
    }

    /// <summary>
    /// 巡邏行為
    /// </summary>
    private void PatrolBehavior()
    {
        Vector3 nextWaypointPostion = beginPostion;
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                if (roarrate <= -1.37f)
                {
                    mutantAudio.MutantRoar(gameObject);
                    roarrate = 2.0f;
                }
                mover.CancelMove();
                animator.SetBool("IsConfuse", true);
                timeSinceArriveWayPoint = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumber(currentWaypointIndex);
            }

            if (timeSinceArriveWayPoint > waypointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex));
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(beginPostion);
        }
    }

    /// <summary>
    /// 檢查是否已經抵達WayPoint
    /// </summary>
    /// <returns>傳回是否已經抵達WayPoint</returns>
    private bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWaypointIndex)) < waypointToStay);
    }

    /// <summary>
    /// 困惑的動作行為
    /// </summary>
    private void ConfuseBehavior()
    {
        mover.CancelMove();
        fighter.CancelTarget();

        // 困惑動作
        animator.SetBool("IsConfuse", true);
    }

    /// <summary>
    /// 判斷是否與玩家的距離小於追趕距離內
    /// </summary>
    /// <returns>傳回是否與玩家的距離小於追趕距離內</returns>
    private bool IsRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    /// <summary>
    /// 計時器
    /// </summary>
    private void UpdateTimer()
    {
        roarrate -= Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArriveWayPoint += Time.deltaTime;
    }

    #region -- 事件相關 --

    /// <summary>
    /// Mutant受到攻擊時處理方法
    /// </summary>
    private void OnDamage()
    {
        //  受到攻擊時，觸發的事件
        isBeHit = true;
    }

    /// <summary>
    /// Mutant死亡時處理方法
    /// </summary>
    private void OnDie()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
        collider.enabled = false;
    }

    #endregion

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    #endregion

}
