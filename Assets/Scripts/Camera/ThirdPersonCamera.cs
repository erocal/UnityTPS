using DG.Tweening;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using static GameManagerSingletonHelper;

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

    [SerializeField] private PlayableDirector playableDirector;

    [Header("Camera")]
    [SerializeField] private Camera miniMapCamera;

    #endregion

    #region -- 變數參考區 --

    #region -- 常數 --

    private const int ORIGINAL_RENDERER = 0;
    private const int RADIAL_BLUR_RENDERER = 2;
    private const int ONE_THOUSAND_MILLISECONDS = 1000;
    private const string ANIMATOR_START_WALK = "StartWalk";
    private const string ANIMATOR_WALK_SPEED = "WalkSpeed";

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

    // 滑鼠是不是鎖住的狀態
    bool isLocked = false;

    Material bloodLossMat;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        Init();

    }

    void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {

        SetBloodLossMaterial();

    }

    private void Update()
    {

        if (CheckOrganismNull(ref organism)) return;

        CheckVolumeMute();

        BloodLossEffectUpdate();

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

        var instance = GameManagerSingleton.Instance;

        input = instance.InputController;
        actionSystem = instance.ActionSystem;
        organism = instance.Organism;
        audioSource = GetComponent<AudioSource>();
        mainCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();

        #region -- 訂閱 --

        actionSystem.OnDamage += OnDamage;
        actionSystem.OnCaplock += OnCaplock;
        actionSystem.OnCaplockUp += OnCaplockUp;
        actionSystem.OnCameraVolumeChange += VolumeUpdate;
        actionSystem.OnLoginCameraMove += LoginCameraMove;
        actionSystem.OnGameStart += GameStart;

        #endregion

        SetMinimap();

        SetBloodLossMaterial();

    }

    /// <summary>
    /// 確認目前音量是否為零，為零切換UI
    /// </summary>
    private void CheckVolumeMute()
    {

        actionSystem.CameraVolumeMute(audioSource.volume == 0);

    }

    /// <summary>
    /// 血量流失特效
    /// </summary>
    private void BloodLossEffectUpdate()
    {

        if (bloodLossMat != null)
        {
            bloodLossMat.SetFloat("_BloodLossIntensity", 1 - (organism.PlayerData.PlayerHealth.currentHealth / organism.PlayerData.PlayerHealth.maxHealth));
        }

    }

    #endregion

    #region -- 事件相關 --

    /// <summary>
    /// 玩家受傷時處理方法
    /// </summary>
    private async void OnDamage(int id)
    {

        if (id != organism.PlayerData.InstanceID) return;

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

    private void LoginCameraMove()
    {

        playableDirector.Play();

        LoginPlayerWalking();

    }

    private async void LoginPlayerWalking()
    {

        var animator = organism.LoginPlayer.GetComponent<Animator>();

        animator.SetTrigger(ANIMATOR_START_WALK);

        await Task.Delay(ONE_THOUSAND_MILLISECONDS);

        DOTween.To(() => animator.GetFloat(ANIMATOR_WALK_SPEED),
           x => animator.SetFloat(ANIMATOR_WALK_SPEED, x),
           0.8f, 2f);

    }

    private void GameStart()
    {

        playableDirector.enabled = false;
        this.GetComponent<Animator>().enabled = false;

        actionSystem.MinimapInit(miniMapCamera.targetTexture);

    }

    private void SetMinimap()
    {

        RenderTexture renderTexture = new RenderTexture(256, 256, 1, RenderTextureFormat.Default);
        renderTexture.name = "Minimap";
        renderTexture.Create();

        miniMapCamera.targetTexture = renderTexture;

    }

    private void SetBloodLossMaterial()
    {

        if (mainCameraData != null)
        {

            // 使用反射取得 rendererFeatures
            FieldInfo fieldInfo = typeof(ScriptableRenderer).GetField("m_RendererFeatures", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                if (fieldInfo.GetValue(mainCameraData.scriptableRenderer) is List<ScriptableRendererFeature> features)
                {
                    foreach (var feature in features)
                    {
                        if (feature is FullScreenPassRendererFeature fullScreenFeature && fullScreenFeature.name == "BloodLossPassRendererFeature")
                        {

                            bloodLossMat = fullScreenFeature.passMaterial;

                        }
                    }
                }
            }

        }
    }

    #endregion

}
