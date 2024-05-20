using UnityEngine;

public abstract class AIController : MonoBehaviour
{
    #region -- 資源參考區 --

    [Tooltip("追趕距離")]
    [SerializeField] protected float chaseDistance = 10f;
    [Tooltip("失去目標後困惑的時間")]
    [SerializeField] protected float confuseTime = 5f;

    [Header("巡邏路徑")]
    [SerializeField] protected PatrolPath patrol;
    [Header("需要到達WayPoint的距離")]
    [SerializeField] protected float waypointToStay = 3f;
    [Header("待在WayPoint的時間")]
    [SerializeField] protected float waypointToWaitTime = 3f;
    [Header("巡邏時的速度")]
    [Range(0, 1)]
    [SerializeField] protected float patrolSpeedRatio = 0.5f;

    [Space(20)]
    [Header("要銷毀的gameobject根節點")]
    [SerializeField] protected GameObject enemyRoot;

    [Space(20)]
    [Header("AI是否受擊"), ReadOnly]
    [SerializeField] protected bool isBeHit;

    [Header("玩家")]
    [SerializeField] protected GameObject player;
    [Header("動畫控制器")]
    [SerializeField] protected Animator animator;
    [Header("Health")]
    [SerializeField] protected Health health;
    [Header("碰撞體")]
    [SerializeField] protected Collider collider;

    #endregion

    #region -- 變數參考區 --

    [Tooltip("上次看到玩家的時間"), ReadOnly]
    [SerializeField] protected float sinceLastSawPlayerTimer = Mathf.Infinity;
    [Tooltip("初始生成座標"), ReadOnly]
    [SerializeField] protected Vector3 aiSpawnPostion;
    [Tooltip("當前需要到達的WayPoint編號"), ReadOnly]
    [SerializeField] protected int currentWaypointIndex = 0;
    [Tooltip("距離上次抵達WayPoint的時間"), ReadOnly]
    [SerializeField] protected float sinceArriveWayPointTimer = 0;

    #endregion

    #region -- 初始化/運作 --

    private void Update()
    {
        if (health.IsDead()) return;
    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 設定AI初始生成位置
    /// </summary>
    /// <param name="spawnPostion">初始生成位置</param>
    protected void SetSpawnPosition(Vector3 spawnPostion)
    {
        this.aiSpawnPostion = spawnPostion;
    }

    /// <summary>
    /// AI所能做出的行為
    /// </summary>
    protected abstract void AIBehavior();

    /// <summary>
    /// 處理AI攻擊行為
    /// </summary>
    protected abstract void AttackBehavior();

    // 巡邏行為
    protected abstract void PatrolBehavior();

    // 困惑的動作行為
    protected abstract void ConfuseBehavior();

    /// <summary>
    /// 看見玩家
    /// </summary>
    protected void SawPlayer()
    {
        sinceLastSawPlayerTimer = 0;
    }

    // 檢查是否已經抵達WayPoint
    protected bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWaypointIndex)) < waypointToStay);
    }

    /// <summary>
    /// 是否小於追趕距離內
    /// </summary>
    protected bool IsRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    /// <summary>
    /// 確認是否受擊
    /// </summary>
    protected bool CheckIsBeHit()
    {
        return isBeHit;
    }

    protected void UpdateLastSawPlayerTimer()
    {
        sinceLastSawPlayerTimer += Time.deltaTime;
    }

    protected void UpdateArriveWayPointTimer()
    {
        sinceArriveWayPointTimer += Time.deltaTime;
    }

    #region -- 事件相關 --

    /// <summary>
    /// AI受到攻擊時處理方法
    /// </summary>
    protected void OnDamage()
    {
        //  受到攻擊時，觸發的事件
        isBeHit = true;
    }

    /// <summary>
    /// AI死亡時處理方法
    /// </summary>
    protected abstract void OnDie();

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
