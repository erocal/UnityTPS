using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFighter : MonoBehaviour
{
    [Header("角色攻擊類型")]
    [SerializeField] Actor actorType;
    [Header("攻擊傷害")]
    [SerializeField] float attackDamage = 10f;
    [Header("跳躍攻擊傷害")]
    [SerializeField] float jumpAttackDamage = 10f;
    [Header("攻擊距離")]
    [SerializeField] float attackRange = 2f;
    [Header("跳躍攻擊距離")]
    [SerializeField] float jumpAttackRange = 2f;
    [Header("攻擊時間間隔")]
    [SerializeField] float timeBetweenAttack = 2f;

    [Space(20)]
    [Header("要丟出去的Projectile")]
    [SerializeField] Projectile throwProjectile;
    [Header("手部座標")]
    [SerializeField] Transform hand;

    RootMover mover;
    Animator animator;
    MutantAudio mutantAudio;
    Health health;
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    // 計時器
    private float attackrate;

    // Start is called before the first frame update
    void Start()
    {
        mover = GetComponent<RootMover>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        mutantAudio = GetComponent<MutantAudio>();
        health.onDie += OnDie;
    }

    // Update is called once per frame
    void Update()
    {
        attackrate -= Time.deltaTime;

        if (targetHealth == null || targetHealth.IsDead()) return;

        if (IsInAttackRange())
        {
            mover.CancelMove();
            AttackBehavior("Attack");
        }

        else if (IsInJumpAttackRange())
        {
            mover.CancelMove();
            AttackBehavior("JumpAttack");
        }

        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position);
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

    private void Hit()
    {
        if (targetHealth == null || actorType != Actor.Mutant) return;

        if (IsInAttackRange())
        {
            if (attackrate <= 0.661f)
            {
                mutantAudio.mutantattack(gameObject);
                attackrate = 2.0f;
            }
            targetHealth.TakeDamage(attackDamage);
        }
    }

    private void JumpHit()
    {
        if (targetHealth == null || actorType != Actor.Mutant) return;

        if (IsInAttackRange())
        {
            if (attackrate <= 0.661f)
            {
                mutantAudio.mutantattack(gameObject);
                attackrate = 2.0f;
            }
            targetHealth.TakeDamage(jumpAttackDamage);
        }
    }

    private void Shoot()
    {
        if (targetHealth == null || actorType != Actor.Archer) return;

        if (throwProjectile != null)
        {
            Projectile newProjectile = Instantiate(throwProjectile, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }

    private bool IsInJumpAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < jumpAttackRange;
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;
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
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpAttackRange);
    }
}
