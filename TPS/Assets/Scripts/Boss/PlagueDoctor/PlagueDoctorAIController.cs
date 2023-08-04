using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorAIController : MonoBehaviour
{
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


    GameObject player;
    //GameObject zombieaudio;
    PlagueDoctorMover mover;
    Animator animator;
    Health health;
    PlagueDoctorFighter fighter;
    Collider collider;
    //ZombieAudio zombieAudio;


    // 上次看到玩家的時間
    private float timeSinceLastSawPlayer = Mathf.Infinity;
    // 原點座標
    private Vector3 beginPostion;
    // 當前需要到達的WayPoint編號
    int currentWaypointIndex = 0;
    // 距離上次抵達WayPoint的時間
    float timeSinceArriveWayPoint = 0;
    // 計時器
    private float zombierate = 2.0f;

    bool isBeHit;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mover = GetComponent<PlagueDoctorMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        fighter = GetComponent<PlagueDoctorFighter>();
        collider = GetComponent<Collider>();

        beginPostion = transform.position;
        health.onDamage += OnDamage;
        health.onDie += OnDie;
    }

    private void Update()
    {
        /*print("當前血量為 : " + health.GetCurrentHealth());
        if (Input.GetKeyDown(KeyCode.S))
        {
            health.TakeDamage(10);
        }*/
        //zombierate -= Time.deltaTime;

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

    private void AttackBehavior()
    {
        animator.SetBool("IsConfuse", false);
        timeSinceLastSawPlayer = 0;
        /*if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombierate <= 0)
        {
            zombieAudio = GetComponent<ZombieAudio>();
            zombieAudio.zombiefollow(gameObject);
            zombierate = 2.0f;
        }*/
        fighter.Attack(player.GetComponent<Health>());
    }

    // 巡邏行為
    private void PatrolBehavior()
    {
        Vector3 nextWaypointPostion = beginPostion;
        if (patrol != null)
        {
            if (IsAtWayPoint())
            {
                mover.CancelMove();
                animator.SetBool("IsConfuse", true);
                timeSinceArriveWayPoint = 0;
                currentWaypointIndex = patrol.GetNextWayPointNumber(currentWaypointIndex);
            }

            if (timeSinceArriveWayPoint > waypointToWaitTime)
            {
                animator.SetBool("IsConfuse", false);
                mover.MoveTo(patrol.GetWayPointPosition(currentWaypointIndex), patrolSpeedRatio);
            }
        }
        else
        {
            animator.SetBool("IsConfuse", false);
            mover.MoveTo(beginPostion, 0.5f);
        }
    }

    // 檢查是否已經抵達WayPoint
    private bool IsAtWayPoint()
    {
        return (Vector3.Distance(transform.position, patrol.GetWayPointPosition(currentWaypointIndex)) < waypointToStay);
    }

    // 困惑的動作行為
    private void ConfuseBehavior()
    {
        mover.CancelMove();
        fighter.CancelTarget();

        // 困惑動作
        animator.SetBool("IsConfuse", true);
    }

    // 是否小於追趕距離內
    private bool IsRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
    }

    private void UpdateTimer()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArriveWayPoint += Time.deltaTime;
    }

    private void OnDamage()
    {
        isBeHit = true;

    }

    private void OnDie()
    {
        mover.CancelMove();
        animator.SetTrigger("IsDead");
        collider.enabled = false;
        //Destroy(enemyRoot);
    }

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }

}
