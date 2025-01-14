using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("HealthBar")]
    [SerializeField] private bool isPlayer;
    [HideIf("isPlayer"), SerializeField] private GameObject rootCanvas;
    [HideIf("isPlayer"), SerializeField] private Image foreground;
    [HideIf("isPlayer"), SerializeField, Range(0, 1)] float changeHealthRatio = 0.1f;

    #endregion

    #region -- 變數參考區 --

    ActionSystem actionSystem;

    AudioSource audioSource;

    // 判斷擁有血量的物體是否處於死亡狀態
    private bool isDead = false;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;

    }

    void Start()
    {

        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {

        rate -= Time.deltaTime;
        zombierate -= Time.deltaTime;

        HealthBar();

    }

    #endregion

    #region -- 方法參考區 --

    private void HealthBar()
    {

        if (isPlayer) return;

        // 如果血量百分比約等於0或是1
        if (Mathf.Approximately(GetHealthRatio(), 0) || Mathf.Approximately(GetHealthRatio(), 1))
        {
            rootCanvas.SetActive(false);
            return;
        }

        rootCanvas.SetActive(true);
        rootCanvas.transform.LookAt(Camera.main.transform.position);
        foreground.fillAmount = Mathf.Lerp(foreground.fillAmount, GetHealthRatio(), changeHealthRatio);

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
            rate = 2.0f;
            if (gothurtSFX != null)
            {
                audioSource.PlayOneShot(gothurtSFX);
            }
        }

        if ((gameObject.tag == "Zombie" || gameObject.tag == "Zombiegrounp") && zombierate <= 0)
        {
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
            actionSystem.Damage(gameObject.GetInstanceID());
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
            actionSystem.Death(this.gameObject.GetInstanceID());
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

    public void AddMaxHealth(float amount)
    {
        if (lifeParticle != null)
            lifeParticle.Play();

        maxHealth += amount;
        currentHealth += amount;
    }

    public void ReBorn()
    {
        isDead = false;
        Heal(maxHealth);
    }

    #endregion

}
