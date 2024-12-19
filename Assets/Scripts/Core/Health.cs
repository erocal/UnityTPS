using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("血量")]
    [SerializeField, Tooltip("最大血量")] public float maxHealth = 10f;
    [SerializeField, Tooltip("當前血量")] public float currentHealth;

    [Space(5)]
    [Header("受傷的音效")]
    [SerializeField] AudioClip gothurtSFX;
    [Header("等待秒數")]
    [SerializeField] float rate = 2.0f;
    [SerializeField] float zombierate = 2.0f;

    [Header("特效")]
    [SerializeField, Tooltip("治療時的特效")] ParticleSystem healParticle;
    [SerializeField, Tooltip("生命提升時的特效")] ParticleSystem lifeParticle;

    #endregion

    #region -- 變數參考區 --

    AudioSource audioSource;

    // 當受到攻擊時觸發的委派事件
    public event Action OnDamage;
    // 當人物死亡時觸發的委派事件
    public event Action OnDie;
    // 判斷擁有血量的物體是否處於死亡狀態
    private bool isDead = false;

    #endregion

    #region -- 初始化/運作 --

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        rate -= Time.deltaTime;
        zombierate -= Time.deltaTime;
    }

    #endregion

    #region -- 方法參考區 --

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthRatio()
    {
        return currentHealth / maxHealth;
    }

    /// <summary>
    /// 判斷呼叫的對象是否已經死亡
    /// </summary>
    /// <returns>傳回呼叫的對象是否已經死亡</returns>
    public bool IsDead()
    {
        return isDead;
    }

    /// <summary>
    /// 處理承受傷害的方法
    /// </summary>
    /// <param name="damage">受到的傷害數值</param>
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        if (gameObject.tag == "Player" && rate <= 0)
        {
            //print("Player目前血量 : " + currentHealth);
            rate = 2.0f;
            if (gothurtSFX != null)
            {
                audioSource.PlayOneShot(gothurtSFX);
            }
        }

        if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombierate <= 0)
        {
            //print("Player目前血量 : " + currentHealth);
            zombierate = 2.0f;
            if (gothurtSFX != null)
            {
                audioSource.PlayOneShot(gothurtSFX);
            }
        }

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth > 0)
        {
            // ? = if (OnDamage != null) //讓他不要重複Invoke
            OnDamage?.Invoke();
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (isDead) return;

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDie?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (healParticle != null)
            healParticle.Play();

        currentHealth += amount;

        // 限制不要加超過
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void Life(float amount)
    {
        if (lifeParticle != null)
            lifeParticle.Play();

        maxHealth += amount;
        currentHealth += amount;
    }

    public void Alive()
    {
        isDead = false;
        Heal(maxHealth);
    }

    #endregion

}
