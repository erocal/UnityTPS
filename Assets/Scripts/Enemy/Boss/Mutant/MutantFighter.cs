﻿using UnityEngine;

public class MutantFighter : MonoBehaviour
{

    #region -- 資源參考區 --

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

    #endregion

    #region -- 參數參考區 --

    Organism organism;

    /// <summary> 追趕的目標人物生命 </summary>
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    float timeSinceLastAttack = Mathf.Infinity;

    // 計時器
    private float attackRate;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        organism = GameManagerSingleton.Instance.Organism;

    }

    void Update()
    {

        MutantMover mover = (MutantMover)organism.MutantData.BossMover;

        attackRate -= Time.deltaTime;

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

    // Called by Unity
    // 這是自行繪製visable可視化物件，用來設計怪物追蹤玩家的範圍
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpAttackRange);

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 檢查攻擊動作是否已經結束
    /// </summary>
    private bool CheckHasAttack()
    {

        baseLayer = organism.MutantData.BossAnimator.GetCurrentAnimatorStateInfo(0);

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

    /// <summary>
    /// 計時器
    /// </summary>
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

        var animator = organism.MutantData.BossAnimator;

        animator.ResetTrigger(attackName);
        animator.SetTrigger(attackName);

    }

    private void Hit()
    {

        if (targetHealth == null || actorType != Actor.Mutant) return;

        if (IsInAttackRange())
        {
            if (attackRate <= 0.661f)
            {

                MutantAIController aiController = (MutantAIController)organism.MutantData.BossAIController;
                aiController.MutantAttack();
                attackRate = 2.0f;

            }
            targetHealth.TakeDamage(attackDamage);
        }

    }

    private void JumpHit()
    {

        if (targetHealth == null || actorType != Actor.Mutant) return;

        if (IsInAttackRange())
        {
            if (attackRate <= 0.661f)
            {

                MutantAIController aiController = (MutantAIController)organism.MutantData.BossAIController;
                aiController.MutantAttack();
                attackRate = 2.0f;

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
            newProjectile.Shoot(organism.MutantData.Boss);
        }

    }

    /// <summary>
    /// 是否在Mutant的跳躍攻擊範圍內
    /// </summary>
    /// <returns>回傳是否在Mutant的跳躍攻擊範圍內</returns>
    private bool IsInJumpAttackRange()
    {

        return Vector3.Distance(transform.position, targetHealth.transform.position) < jumpAttackRange;

    }

    /// <summary>
    /// 是否在Mutant的一般攻擊範圍內
    /// </summary>
    /// <returns>回傳是否在Mutant的一般攻擊範圍內</returns>
    private bool IsInAttackRange()
    {

        return Vector3.Distance(transform.position, targetHealth.transform.position) < attackRange;

    }

    /// <summary>
    /// 攻擊目標
    /// </summary>
    /// <param name="target">目標</param>
    public void Attack(Health target)
    {

        targetHealth = target;

    }

    /// <summary>
    /// 取消目前追蹤的攻擊目標
    /// </summary>
    public void CancelTarget()
    {

        targetHealth = null;

    }

    #endregion

}
