using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponShootType
{
    Single,
    Automatic,
}

public class WeaponController : MonoBehaviour
{
    [Header("Icon")]
    public Sprite weaponIcon;

    [Header("武器的主要GameObject，不使用時將被隱藏")]
    [SerializeField] GameObject weaponRoot;
    [Header("槍口位置，發射子彈的位置")]
    [SerializeField] Transform weaponMuzzle;

    [Space(5)]
    [Header("射擊形式")]
    [SerializeField] WeaponShootType shootType;
    [Header("Projectile Prefab")]
    [SerializeField] Projectile projectilePrefab;
    [Header("兩次射擊之間的Delay時間")]
    [SerializeField] float delayBetweenShoots = 0.5f;
    [Header("射一發所需要的子彈數量")]
    [SerializeField] int bulletPerShoot;

    [Space(5)]
    [Header("每秒Reload彈藥的數量")]
    [SerializeField] float ammoReloadRate = 1f;
    [Header("射擊完畢到可以Reload的Delay時間")]
    [SerializeField] float ammoReloadDelay = 2f;
    [Header("最大子彈數量")]
    [SerializeField] float maxAmmo = 8;

    [Space(5)]
    [Header("槍口發射時產生的特效")]
    [SerializeField] GameObject muzzleFlashPrefab;
    [Header("Shoot的音效")]
    [SerializeField] AudioClip shootSFX;
    [Header("切換到這個武器時的音效")]
    [SerializeField] AudioClip changeWeaponSFX;

    public GameObject sourcePrefab { get; set; }
    public float currentAmmoRatio { get; private set; }
    public bool isCooling { get; private set; }

    AudioSource audioSource;

    // 當前子彈數量
    float currentAmmo;
    // 距離上次射擊的時間
    float timeSinceLastShoot;
    // 是否在瞄準狀態
    bool isAim;

    ProjectilePool projectilePool = null;

    private void Awake()
    {
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();

        projectilePool = GameObject.Find(ProjectilePool.PROJECTILEPOOLNAME).GetComponent<ProjectilePool>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmo();
    }

    private void UpdateAmmo()
    {
        if (timeSinceLastShoot + ammoReloadDelay < Time.time && currentAmmo < maxAmmo)
        {
            //當前子彈開始Reload
            currentAmmo += ammoReloadRate;

            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

            isCooling = true;
        }
        else
        {
            isCooling = false;
        }

        if (maxAmmo == Mathf.Infinity)
        {
            currentAmmoRatio = 1f;
        }
        else
        {
            currentAmmoRatio = currentAmmo / maxAmmo;
        }
    }

    public void ShowWeapon(bool value)
    {
        weaponRoot.SetActive(value);

        if (value && changeWeaponSFX)
        {
            audioSource.PlayOneShot(changeWeaponSFX);
        }
    }

    public void HandleShootInput(bool inputDown, bool inputHeld, bool inputUp)
    {
        switch (shootType)
        {
            case WeaponShootType.Single:
                if (inputDown)
                {
                    TryShoot();
                }
                return;
            case WeaponShootType.Automatic:
                if (inputHeld)
                {
                    TryShoot();
                }
                return;
            default:
                return;
        }
    }

    /// <summary>
    /// 判斷彈藥量是否足夠射擊
    /// </summary>
    private void TryShoot()
    {
        if (currentAmmo >= 1f && timeSinceLastShoot + delayBetweenShoots < Time.time)
        {
            HandleShoot();
            currentAmmo -= 1;
        }
    }

    private void HandleShoot()
    {
        for (int i = 0; i < bulletPerShoot; i++)
        {
            Projectile newProjectile = null;

            if (projectilePrefab.projectileId == ProjectileId.None)
                newProjectile = Instantiate(projectilePrefab, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            else
                newProjectile = projectilePool.ReUse(projectilePrefab.projectileId, weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward));
            
            newProjectile.Shoot(GameObject.FindGameObjectWithTag("Player"));
        }

        if (muzzleFlashPrefab != null)
        {
            GameObject newMuzzlePrefab = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
            Destroy(newMuzzlePrefab, 1.5f);
        }

        if (shootSFX != null)
        {
            audioSource.PlayOneShot(shootSFX);
        }

        timeSinceLastShoot = Time.time;
    }

    /// <summary>
    /// 恢復指定數量的子彈數
    /// </summary>
    /// <param name="amount">恢復的子彈量</param>
    public void fullammo(float amount)
    {
        currentAmmo += amount;

        // 限制不要加超過
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);
    }
}
