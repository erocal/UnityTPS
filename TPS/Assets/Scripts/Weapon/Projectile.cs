using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Coliider,
    Particle,
}

public class Projectile : MonoBehaviour
{
    [Header("Type")]
    [SerializeField] ProjectileType type;
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

    GameObject owner;
    bool canAttack;

    // 子彈當前飛行速度
    Vector3 currentVelocity;

    private void OnEnable()
    {
        Destroy(gameObject, maxLifetime);
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
        if (other.gameObject == owner || !canAttack) return;

        if ((other.gameObject.tag == "Zombiegrounp" || other.gameObject.tag == "Zombie" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && type == ProjectileType.Coliider)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
        Destroy(gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other == owner || !canAttack) return;

        if ((other.gameObject.tag == "Zombiegrounp" || other.gameObject.tag == "Zombie" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player") && type == ProjectileType.Particle)
        {
            Health targetHealth = other.gameObject.GetComponent<Health>();
            if (!targetHealth.IsDead())
            {
                targetHealth.TakeDamage(damage);
            }
        }

        HitEffect(transform.position);
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

    public void Shoot(GameObject gameObject)
    {
        owner = gameObject;
        currentVelocity = transform.forward * projectileSpeed;
        canAttack = true;
    }
}
