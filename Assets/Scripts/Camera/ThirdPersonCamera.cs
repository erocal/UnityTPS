using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ThirdPersonCamera : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("Camera跟隨的目標")]
    [SerializeField] Transform target;

    [Header("水平軸靈敏度")]
    [SerializeField] float sensitivity_X = 2;
    [Header("垂直軸靈敏度")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensitivity_Z = 5;

    [Header("最小垂直角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("最大垂直角度")]
    [SerializeField] float maxVerticalAngle = 85;
    [Header("相機與目標的距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("最小相機與目標的距離")]
    [SerializeField] float minDistance = 2;
    [Header("最大相機與目標的距離")]
    [SerializeField] float maxDistance = 25;
    [Header("受傷時的特效")]
    [SerializeField] ParticleSystem beHitParticle;
    [Header("加速時的特效")]
    [SerializeField] ParticleSystem caplockParticle;

    [Header("Pause的音效")]
    [SerializeField] AudioClip pauseSFX;

    [Header("Offset")]
    [SerializeField] Vector3 offset;

    #endregion

    #region -- 變數參考區 --

    #region -- 常數 --

    private const int ORIGINAL_RENDERER = 0;
    private const int RADIAL_BLUR_RENDERER = 1;

    #endregion

    Organism organism;

    InputController input;
    ActionSystem actionSystem;
    AudioSource audioSource;
    UniversalAdditionalCameraData mainCameraData;

    float mouse_X = 0;
    float mouse_Y = 30;

    bool isRidalBlur;

    bool isChange;

    // 應該是看滑鼠是不是鎖住的狀態
    bool isLocked = false;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        Init();

    }

    private void Update()
    {

        CheckVolumeMute();

    }

    private void LateUpdate()
    {

        if (Cursor.lockState == CursorLockMode.Locked)
        {

            mouse_X += input.GetMouseXAxis() * sensitivity_X;
            mouse_Y += input.GetMouseYAxis() * sensitivity_Y;

            // 限制垂直角度
            mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);

            // 計算旋轉角度
            Quaternion rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);

            // 計算相機位置
            Vector3 position = rotation * new Vector3(0, 0, -cameraToTargetDistance)
                               + target.position
                               + Vector3.up * offset.y;

            // 固定相機在玩家身後
            transform.SetPositionAndRotation(position, rotation);

            // 滑鼠滾輪控制遠近
            cameraToTargetDistance += input.GetMouseScrollWheelAxis() * sensitivity_Z;
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, minDistance, maxDistance);

            isLocked = false;
        }
        else
        {

            isLocked = true;

        }

        if (isLocked != isChange)
        {
            audioSource.PlayOneShot(pauseSFX);
            isChange = isLocked;
        }

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化參數
    /// </summary>
    private void Init()
    {

        organism = Organism.Instance;

        var instance = GameManagerSingleton.Instance;

        input = instance.InputController;
        actionSystem = instance.ActionSystem;
        audioSource = GetComponent<AudioSource>();
        mainCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();

        #region -- 訂閱 --

        actionSystem.OnDamage += OnDamage;
        actionSystem.OnCaplock += OnCaplock;
        actionSystem.OnCaplockUp += OnCaplockUp;
        actionSystem.OnCameraVolumeChange += VolumeUpdate;

        #endregion

    }

    /// <summary>
    /// 確認目前音量是否為零，為零切換UI
    /// </summary>
    private void CheckVolumeMute()
    {

        actionSystem.CameraVolumeMute(audioSource.volume == 0);

    }

    #endregion

    #region -- 事件相關 --

    /// <summary>
    /// 玩家受傷時處理方法
    /// </summary>
    private async void OnDamage(int id)
    {

        if (id != organism.GetPlayer().GetInstanceID()) return;

        beHitParticle.Play();

        actionSystem.AnimatorTriggerDamage(id, "IsDamage");
        await Task.Delay(2000);

    }

    /// <summary>
    /// 玩家加速時處理方法
    /// </summary>
    private async void OnCaplock()
    {
        if (caplockParticle == null) return;
        caplockParticle.Play();

        if (mainCameraData != null)
        {

            if (!isRidalBlur)
            {

                CameraSetRenderer(RADIAL_BLUR_RENDERER);

                isRidalBlur = true;

                await Task.Delay(1000);

                CameraSetRenderer(ORIGINAL_RENDERER);

            }

        }

    }

    /// <summary>
    /// 相機設置渲染資料
    /// </summary>
    private void CameraSetRenderer(int renderer)
    {

        if (mainCameraData != null)
            mainCameraData.SetRenderer(renderer);

    }

    /// <summary>
    /// 玩家鬆開加速鍵時處理方法
    /// </summary>
    private void OnCaplockUp()
    {

        isRidalBlur = false;

    }

    /// <summary>
    /// 音量
    /// </summary>
    private void VolumeUpdate(float volume)
    {

        audioSource.volume = volume;

    }

    #endregion

}
