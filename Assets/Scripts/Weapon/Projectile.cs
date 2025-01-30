using System.Collections;
using ToolBox.Pools;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("Type")]
    [SerializeField] ProjectileType type;
    [Header("子彈軌跡")]
    public TrailRenderer trailRenderer;
    [Header("射到目標的Particle")]
    [SerializeField] GameObject hitParticle;
    [Header("擊中物件特效的存活時間")]
    [SerializeField] float particleLifeTime = 2f;
    [Header("子彈速度")]
    [SerializeField] float projectileSpeed;


    [Header("Projectile的存活時間")]
    [SerializeField] float maxLifetime = 3f;
    [Header("重力(影響子彈下墜的力量)")]
    [SerializeField] float gravityDownForce = 0f;

    [SerializeField] float damage = 40f;

    #endregion

    #region -- 變數參考區 --

    PoolInstaller poolInstaller;
    GameObject owner;
    bool canAttack;

    // 子彈當前飛行速度
    Vector3 currentVelocity;

    TagHandle zombieTagHandle;
    TagHandle zombiegrounpTagHandle;
    TagHandle enemyTagHandle;
    TagHandle playerTagHandle;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        poolInstaller = GameManagerSingleton.Instance.PoolInstaller;

        zombieTagHandle = TagHandle.GetExistingTag("Zombie");
        zombiegrounpTagHandle = TagHandle.GetExistingTag("Zombiegrounp");
        enemyTagHandle = TagHandle.GetExistingTag("Enemy");
        playerTagHandle = TagHandle.GetExistingTag("Player");

    }

    private void OnEnable()
    {

        // maxLifetime秒後回收子彈
        StartCoroutine(RecoverAfterDelay(maxLifetime));

    }

    
    void Update()
    {

        transform.position += currentVelocity * Time.deltaTime;

        if (gravityDownForce > 0)
        {
            currentVelocity += gravityDownForce * Time.deltaTime * Vector3.down;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == owner || !canAttack || LayerMask.LayerToName(other.gameObject.layer) == "MapAreaTrigger") return;

        TargetTakeDamage(other, ProjectileType.Coliider);

        HitEffect(transform.position);

        if (trailRenderer != null)
            trailRenderer.enabled = false;

        this.gameObject.Release(poolInstaller.transform);

    }

    private void OnParticleCollision(GameObject other)
    {

        if (other == owner || !canAttack) return;

        TargetTakeDamage(other, ProjectileType.Particle);

        HitEffect(transform.position);

    }

    #endregion

    #region -- 方法參考區 --

    private void TargetTakeDamage<T>(T target, ProjectileType projectileType)where T : class
    {

        GameObject other;

        // 檢查T的類型
        if (target is GameObject)
        {
            other = target as GameObject;
        }
        else if (target is Collider)
        {
            other = (target as Collider).gameObject;
        }
        else
        {
            Log.Error("Unsupported type: " + typeof(T).Name);
            return;
        }

        if ( TagCheckOrganismAndEnemy(other) && type == projectileType )
        {

            Health targetHealth = other.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }

        }

    }

    private bool TagCheckOrganismAndEnemy(GameObject other)
    {

        TagHandle otherTagHandle = TagHandle.GetExistingTag(other.tag);
        return otherTagHandle.Equals(zombieTagHandle) || otherTagHandle.Equals(zombiegrounpTagHandle) ||
            otherTagHandle.Equals(enemyTagHandle) || otherTagHandle.Equals(playerTagHandle);

    }

    private void HitEffect(Vector3 hitpoint)
    {

        if (hitParticle)
        {
            GameObject newParticleInstance = Instantiate(hitParticle, hitpoint, transform.rotation);
            if (particleLifeTime > 0)
            {
                Destroy(newParticleInstance, particleLifeTime);
            }
        }

    }

    /// <summary>
    /// 射出子彈的速度
    /// </summary>
    public void Shoot(GameObject owner)
    {

        this.owner = owner;
        currentVelocity = transform.forward * projectileSpeed;
        canAttack = true;

    }

    #region -- 協程 --

    IEnumerator RecoverAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);

        if (trailRenderer != null)
            trailRenderer.enabled = false;

        // 在這裡執行 Recovery 方法
        this.gameObject.Release(poolInstaller.transform);

    }

    #endregion

    #endregion

}
