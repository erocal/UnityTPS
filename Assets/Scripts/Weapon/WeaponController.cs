using UnityEngine;
using ToolBox.Pools;
public class WeaponController : MonoBehaviour
{

    #region -- 資源參考區 --

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

    #endregion

    #region -- 變數參考區 --

    public GameObject SourcePrefab { get; set; }
    public float CurrentAmmoRatio { get; private set; }
    public bool IsCooling { get; private set; }

    private PoolInstaller poolInstaller;
    private Organism organism;
    AudioSource audioSource;

    // 當前子彈數量
    float currentAmmo;
    // 距離上次射擊的時間
    float timeSinceLastShoot;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        poolInstaller = GameManagerSingleton.Instance.PoolInstaller;
        organism = Organism.Instance;
        currentAmmo = maxAmmo;
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        UpdateAmmo();
    }

    #endregion

    #region -- 方法參考區 --

    private void UpdateAmmo()
    {
        if (timeSinceLastShoot + ammoReloadDelay < Time.time && currentAmmo < maxAmmo)
        {
            //當前子彈開始Reload
            currentAmmo += ammoReloadRate;

            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);

            IsCooling = true;
        }
        else
        {
            IsCooling = false;
        }

        if (maxAmmo == Mathf.Infinity)
        {
            CurrentAmmoRatio = 1f;
        }
        else
        {
            CurrentAmmoRatio = currentAmmo / maxAmmo;
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

            newProjectile = projectilePrefab.gameObject.Reuse<Projectile>(weaponMuzzle.position, Quaternion.LookRotation(weaponMuzzle.forward), poolInstaller.transform);

            var trail = newProjectile.trailRenderer;
            if (trail != null)
            {
                trail.Clear(); // 清除軌跡緩存
                trail.enabled = true; // 再次啟用
            }

            newProjectile.Shoot(organism.GetPlayer());

        }

        if (muzzleFlashPrefab != null)
        {
            GameObject newMuzzlePrefab = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle);
            Destroy(newMuzzlePrefab, 1.5f);
        }

        if (shootSFX != null)
            audioSource.PlayOneShot(shootSFX);

        timeSinceLastShoot = Time.time;

    }

    /// <summary>
    /// 恢復指定數量的子彈數
    /// </summary>
    /// <param name="amount">恢復的子彈量</param>
    public void Fullammo(float amount)
    {

        currentAmmo += amount;

        // 限制不要加超過
        currentAmmo = Mathf.Min(currentAmmo, maxAmmo);

    }

    #endregion

}
