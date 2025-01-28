using UnityEngine;

[RequireComponent(typeof(PlagueDoctorAIController))]
public class PlagueDoctorFighter : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("角色攻擊類型")]
    [SerializeField] public Actor actorType;
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

    #endregion

    #region -- 參數參考區 --

    ActionSystem actionSystem;
    Organism organism;

    PlagueDoctorAIController aiController;
    PlagueDoctorMover mover;
    Animator animator;
    Health targetHealth;
    AnimatorStateInfo baseLayer;

    // 計時器
    private float shootrate = 2.0f;
    private float continuousshootrate = 2.0f;

    float timeSinceLastAttack = Mathf.Infinity;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;
        organism = Organism.Instance;
    }

    void Start()
    {

        aiController = GetComponent<PlagueDoctorAIController>();
        mover = GetComponent<PlagueDoctorMover>();
        animator = GetComponent<Animator>();
        actionSystem.OnDie += OnDie;

    }

    void Update()
    {
        shootrate -= Time.deltaTime;
        continuousshootrate -= Time.deltaTime;

        if (targetHealth == null || targetHealth.IsDead()) return;

        if (IsInContinuousShootRange())
        {
            mover.CancelMove();
            AttackBehavior("ContinuousAttack");
        }

        else if (IsInShootRange())
        {
            mover.CancelMove();
            AttackBehavior("Attack");
        }

        else if (CheckHasAttack() && timeSinceLastAttack > timeBetweenAttack)
        {
            mover.MoveTo(targetHealth.transform.position, 1f);
        }

        UpdateTimer();
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

    #endregion

    #region -- 方法參考區 --

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
                aiController.PlagueDoctorFireBall();
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
                aiController.PlagueDoctorLighting();
                continuousshootrate = 2.0f;
            }
            Projectile newProjectile = Instantiate(throwProjectile2, hand.position, Quaternion.LookRotation(transform.forward));
            newProjectile.Shoot(gameObject);
        }
    }

    /// <summary>
    /// 是否處於ContinuousShoot的攻擊範圍內
    /// </summary>
    private bool IsInContinuousShootRange()
    {
        return Vector3.Distance(transform.position, targetHealth.transform.position) < continuousShootRange;
    }

    /// <summary>
    /// 是否處於Shoot的攻擊範圍內
    /// </summary>
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

    private void OnDie(int id)
    {

        if (id != organism.GetPlagueDoctor().GetInstanceID()) return;

        this.enabled = false;

    }

    #endregion

}
