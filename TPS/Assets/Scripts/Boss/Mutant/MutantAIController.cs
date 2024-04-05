using System;
using UnityEngine;

public class MutantAIController : AIController
{
    #region -- 變數參考區 --

    Organism organism;

    MutantMover mutantMover;
    MutantFighter mutantFighter;
    MutantAudio mutantAudio;

    // 計時器
    private float roarTimer = 2.0f;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        organism = Organism.Instance;

        player = organism.GetPlayer();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        collider = GetComponent<Collider>();
        mutantMover = GetComponent<MutantMover>();
        mutantFighter = GetComponent<MutantFighter>();
        mutantAudio = GetComponent<MutantAudio>();

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

    /// <summary>
    /// 攻擊行為
    /// </summary>
    protected override void AttackBehavior()
    {
        animator.SetBool("IsConfuse", false);
        SawPlayer();
        mutantFighter.Attack(player.GetComponent<Health>());
    }

    /// <summary>
    /// 巡邏行為
    /// </summary>
    protected override void PatrolBehavior()
    {
        Vector3 nextWaypointPostion = aiSpawnPostion;
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                if (roarTimer <= -1.37f)
                {
                    mutantAudio.MutantRoar(gameObject);
                    roarTimer = 2.0f;
                }
                mutantMover.CancelMove();
                animator.SetBool("IsConfuse", true);
                sinceArriveWayPointTimer = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumber(currentWaypointIndex);
            }

            if (sinceArriveWayPointTimer > waypointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mutantMover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex));
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mutantMover.MoveTo(aiSpawnPostion);

            try
            {
                patrol = GameObject.Find("PatrolPath_Mutant").GetComponent<PatrolPath>();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    /// <summary>
    /// 困惑的動作行為
    /// </summary>
    protected override void ConfuseBehavior()
    {
        mutantMover.CancelMove();
        mutantFighter.CancelTarget();

        // 困惑動作
        animator.SetBool("IsConfuse", true);
    }

    /// <summary>
    /// 計時器
    /// </summary>
    private void UpdateTimer()
    {
        UpdateLastSawPlayerTimer();
        UpdateArriveWayPointTimer();
    }

    #region -- 事件相關 --

    /// <summary>
    /// Mutant死亡時處理方法
    /// </summary>
    protected override void OnDie()
    {
        mutantMover.CancelMove();
        animator.SetTrigger("IsDead");
        collider.enabled = false;
    }

    #endregion

    #region -- DrawGizmo --

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

    #endregion

    #endregion

}
