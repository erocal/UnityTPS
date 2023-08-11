using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("最大血量")]
    [SerializeField] public float maxHealth = 10f;
    [Header("當前血量")]
    [SerializeField] public float currentHealth;

    [Space(5)]
    [Header("受傷的音效")]
    [SerializeField] AudioClip gothurtSFX;
    [Header("等待秒數")]
    [SerializeField] float rate = 2.0f;
    [SerializeField] float zombierate = 2.0f;

    [Header("治療時的特效")]
    [SerializeField] ParticleSystem healParticle;
    [Header("生命提升時的特效")]
    [SerializeField] ParticleSystem lifeParticle;

    AudioSource audioSource;

    // 當受到攻擊時觸發的委派事件
    public event Action onDamage;
    // 當受到治療效果時，觸發的委派事件，並且回傳float的變數
    public event Action<float> onHealed;
    // 當人物死亡時觸發的委派事件
    public event Action onDie;

    private bool isDead = false;


    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        rate -= Time.deltaTime;
        zombierate -= Time.deltaTime;

        /*if (GameObject.FindWithTag("Player"))
        {
            GameObject player = GameObject.FindWithTag("Player");
            print(player.GetComponent<Health>().currentHealth);
        }*/
    }

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

    public bool IsDead()
    {
        return isDead;
    }

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
            // ? = if (onDamage != null) //讓他不要重複Invoke
            onDamage?.Invoke();
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
            onDie?.Invoke();
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
        /*if (GameObject.FindWithTag("Player"))
        {
            print(this.currentHealth);
        }*/
    }

    public void Alive()
    {
        isDead = false;
        Heal(maxHealth);
    }

}
