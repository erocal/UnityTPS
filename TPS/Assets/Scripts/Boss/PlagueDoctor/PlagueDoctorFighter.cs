using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueDoctorFighter : MonoBehaviour
{
    [Header("角色攻擊類型")]
    [SerializeField] public Actor actorType;
    [Header("攻擊傷害")]
    [SerializeField] float attackDamage = 10f;
    [Header("連續射擊傷害")]
    [SerializeField] float continuousAttackDamage = 10f;
    [Header("攻擊距離")]
    [SerializeField] float shootRange = 2f;
    [Header("連續射擊距離")]
    [SerializeField] float continuousShootRange = 2f;
    [Header("攻擊時間間隔")]
    [SerializeField] float timeBetweenAttack = 2f;

    [Space(20)]
    [Header("要丟出去的Projectile")]
    [SerializeField] Projectile throwProjectile;
    [SerializeField] Projectile throwProjectile2;
    [Header("手部座標")]
    [SerializeField] Transform hand;

    PlagueDoctorMover mover;
    Animator animator;
    Health health;
    Health targetHealth;
    PlagueDoctorAudio plagueDoctorAudio;
    AnimatorStateInfo baseLayer;

    // 計時器
    private float shootrate = 2.0f;
    private float continuousshootrate = 2.0f;

    float timeSinceLastAttack = Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<PlagueDoctorMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        plagueDoctorAudio = GetComponent<PlagueDoctorAudio>();
        health.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        shootrate -= Time.deltaTime;
        continuousshootrate -= Time.deltaTime;

        if (targetHealth == null || targetHealth.IsDead()) return;

        if (IsInContinuousShootRange())
        {
            //print("1");
            mover.CancelMove();
            AttackBehavior("ContinuousAttack");
        }

        else if (IsInShootRange())
        {
            //print("2");
            mover.CancelMove();
            AttackBehavior("Attack");
        }

        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position, 1f);
        }

        UpdateTimer();
    }

    // 檢查攻擊動作是否已經結束
    private bool CheckHasAttack()
    {
        baseLayer = animator.GetCurrentAnimatorStateInfo(0);

        // 如果當前動作等於攻擊
        if (baseLayer.fullPathHash == Animator.StringToHash("Base Layer.Attack"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void UpdateTimer()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    private void AttackBehavior(string attackName)
    {
        transform.LookAt(targetHealth.transform);

        if (timeSinceLastAttack > timeBetweenAttack)
        {
            timeSinceLastAttack = 0;
            TriggerAttack(attackName);
        }
    }

    private void TriggerAttack(string attackName)
    {
        animator.ResetTrigger(attackName);
        animator.SetTrigger(attackName);
    }

    private void Shoot()
    {
        if (targetHealth == null || actorType != Actor.Plague) return;

        if (throwProjectile != null)
        {
            if (shootrate <= 0.594f)
            {
                plagueDoctorAudio.plaguedoctorfireball(gameObject);
                shootrate = 2.0f;
            }
            Projectile newProjectile = Instantiate(throwProjectile, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }

    private void ContinuousShoot()
    {
        if (targetHealth == null || actorType != Actor.Plague) return;

        if (throwProjectile2 != null)
        {
            if (continuousshootrate <= 0)
            {
                plagueDoctorAudio.plaguedoctorlightingball(gameObject);
                continuousshootrate = 2.0f;
            }
            Projectile newProjectile = Instantiate(throwProjectile2, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }

    private bool IsInContinuousShootRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < continuousShootRange;
    }

    private bool IsInShootRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < shootRange;
    }

    public void Attack(Health target)
    {
        targetHealth = target;
    }

    public void CancelTarget()
    {
        targetHealth = null;
    }

    private void OnDie()
    {
        this.enabled = false;
    }

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, continuousShootRange);
    }
}
