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

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        poolInstaller = GameManagerSingleton.Instance.PoolInstaller;

    }

    private void OnEnable()
    {
        // maxLifetime秒後回收子彈
        StartCoroutine(RecoverAfterDelay(maxLifetime));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += currentVelocity * Time.deltaTime;

        if (gravityDownForce > 0)
        {
            currentVelocity += Vector3.down * gravityDownForce * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject == owner || !canAttack || LayerMask.LayerToName(other.gameObject.layer) == "MapAreaTrigger") return;

        if ((other.tag == "Zombiegrounp" || other.tag == "Zombie" || other.tag == "Enemy" || other.tag == "Player") && type == ProjectileType.Coliider)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);

        if (trailRenderer != null)
            trailRenderer.enabled = false;

        this.gameObject.Release(poolInstaller.transform);

    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == owner || !canAttack) return;

        if ((other.tag == "Zombiegrounp" || other.tag == "Zombie" || other.tag == "Enemy" || other.tag == "Player") && type == ProjectileType.Particle)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
    }

    #endregion

    #region -- 方法參考區 --

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
    /// <param name="gameObject"></param>
    public void Shoot(GameObject gameObject)
    {
        owner = gameObject;
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
