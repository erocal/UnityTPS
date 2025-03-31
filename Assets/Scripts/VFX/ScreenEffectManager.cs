using DG.Tweening;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using static GameManagerSingletonHelper;

public class ScreenEffectManager : MonoBehaviour
{

    #region -- 資源參考區 --

    #endregion

    #region -- 變數參考區 --

    #region -- 常數 --

    const float MIN_VIGENETTE_INTENSITY = .7f;
    const float MAX_VIGENETTE_INTENSITY = 3f;

    #endregion

    UniversalAdditionalCameraData mainCameraData;

    ActionSystem actionSystem;
    Organism organism;

    private Tween bloodLossTween;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        mainCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        SetBloodLossMaterial();

    }

    private void Update()
    {

        if (CheckOrganismNull(ref organism)) return;
        BloodLossEffectUpdate();

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化參數
    /// </summary>
    private void Init()
    {

        var instance = GameManagerSingleton.Instance;

        actionSystem = instance.ActionSystem;
        organism = instance.Organism;

        actionSystem.OnDamage += OnDamageBloodLoss;

    }

    private void SetBloodLossMaterial()
    {

        if (mainCameraData == null) return;


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

    /// <summary>
    /// 血量流失特效
    /// </summary>
    private void BloodLossEffectUpdate()
    {

        if (bloodLossMat == null || organism == null) return;

        bloodLossMat.SetFloat("_BloodLossIntensity", 1 - (organism.PlayerData.PlayerHealth.currentHealth / organism.PlayerData.PlayerHealth.maxHealth));

    }

    /// <summary>
    /// 受傷瞬間血量流失特效
    /// </summary>
    private void OnDamageBloodLoss(int id)
    {

        if (bloodLossTween != null && bloodLossTween.IsActive()) return;

        if (id != organism.PlayerData.InstanceID) return;

        bloodLossMat.SetFloat("_VigenetteIntensity", .7f);

        bloodLossTween = DOTween.To(() => bloodLossMat.GetFloat("_VigenetteIntensity"), x => bloodLossMat.SetFloat("_VigenetteIntensity", x), MAX_VIGENETTE_INTENSITY, .4f)
            .SetEase(Ease.InOutSine) // 設置動畫曲線，這裡使用InOutSine來獲得平滑的變化
            .SetLoops(2, LoopType.Yoyo) // 設置動畫循環 LoopType.Yoyo表示來回循環
            .OnComplete(() => bloodLossTween = null); 

    }

    #endregion

}