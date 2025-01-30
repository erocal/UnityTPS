#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UISystem : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("Text")]
    [SerializeField, Tooltip("FPS")] private Text Text_FPS;
    [SerializeField, Tooltip("ms")] private Text Text_ms;

    [Header("等待圖"), Tooltip("切換場景時的等待畫面")]
    [SerializeField] Image loadingImage;

    [Header("Btn")]
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Quit;
    [SerializeField] Button pauseUI_Btn_Quit;
    [SerializeField] Button btn_Volume;
    [SerializeField] Button btn_Mute;
    [SerializeField] Button btn_Continue;
    [SerializeField] Button btn_Reset;
    [SerializeField] Button aliveUI_Btn_Respawn;

    [Header("Slider")]
    [SerializeField] Slider Slider_Music;

    [Header("GameObject")]
    [SerializeField, Tooltip("遊戲開始UI")] GameObject startGameUI;
    [SerializeField, Tooltip("Pause的UI")] GameObject pauseUI;
    [SerializeField, Tooltip("重生的UI")] GameObject aliveUI;
    [SerializeField, Tooltip("音量鍵的UI")] GameObject btnVolumeUI;
    [SerializeField, Tooltip("靜音時的UI")] GameObject btnMuteUI;
    [SerializeField, Tooltip("音量條的UI")] GameObject volumeSliderUI;
    [SerializeField, Tooltip("準星Icon")] GameObject crosshair;

    [Header("Image")]
    [SerializeField] Image healthImage;
    [SerializeField] Image Image_Gazed;

    [Header("WeaponUI")]
    [SerializeField] WeaponUIElements[] weaponUI;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup canvasGroup_StartUI;

    #endregion

    #region -- 變數參考區 --

    public GameObject CrossHair
    {
        get { return crosshair; }
    }

    #region -- 常數 --

    private const int FIVE_THOUSAND_MILLISECONDS = 5000;

    #endregion

    ActionSystem actionSystem;
    InputController input;
    Organism organism;

    private Color32 originalImageGazedColor = new Color32(229, 23, 24, 168);

    private float deltaTime = 0.0f;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        Init();

    }

    private void Update()
    {

        if(CheckOrganismNull()) return;

        CalculateFPSAndMsec();
        PlayerHealthUpdate();
        WeaponUIUpdate();

    }

    private void LateUpdate()
    {

        if (CheckOrganismNull()) return;

        AliveUI();
        PauseUI();

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {

        var instance = GameManagerSingleton.Instance;
        actionSystem = instance.ActionSystem;
        input = instance.InputController;
        organism = instance.Organism;

        actionSystem.OnDie += OnDie;
        actionSystem.OnCameraVolumeMute += VolumeUI;
        actionSystem.OnGazed += ImageGazedChangeColor;
        actionSystem.OnAddWeapon += OnAddWeapon;

        #region -- btn --

        ButtonOnClick();

        #endregion

        Slider_Music.onValueChanged.AddListener(actionSystem.CameraVolumeChange);

    }

    /// <summary>
    /// 玩家死亡時處理方法
    /// </summary>
    private void OnDie(int id)
    {

        if (id != organism.PlayerData.InstanceID) return;

        aliveUI.SetActive(true);
        input.CursorStateChange(false);

    }

    private void ImageGazedChangeColor(bool inGazed)
    {

        Image_Gazed.color = inGazed ? Color.green : originalImageGazedColor;

    }

    private void OnAddWeapon(WeaponController weapon, int index)
    {
        weaponUI[index].weaponIcon.enabled = true;
        weaponUI[index].weaponIcon.sprite = weapon.weaponIcon;
    }

    private void ButtonOnClick()
    {

        btn_Start.onClick.AddListener(async () => await OnStartGame());
        btn_Quit.onClick.AddListener(OnQuitGame);
        pauseUI_Btn_Quit.onClick.AddListener(OnQuitGame);
        btn_Volume.onClick.AddListener(OnVolume);
        btn_Mute.onClick.AddListener(OnVolume);
        btn_Continue.onClick.AddListener(OnContinueGame);
        btn_Reset.onClick.AddListener(OnReset);
        aliveUI_Btn_Respawn.onClick.AddListener(OnRespawn);

    }

    #region -- onClick --

    /// <summary>
    /// Button-Start 加載下一張地圖
    /// </summary>
    private async Task OnStartGame()
    {

        // 加載Game
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            var playerCharacterController = organism.PlayerData.PlayerCharacterController;

            input.CursorStateChange(true);

            Destroy(startGameUI);

            organism.PlayerData.Player.SetActive(true);
            playerCharacterController.enabled = false;

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

            await Task.Delay(FIVE_THOUSAND_MILLISECONDS);

            playerCharacterController.enabled = true;

            actionSystem.SpawnPointUpdate(organism.PlayerData.PlayerController.spawnPos, MapAreaType.StartArea);

            canvasGroup_StartUI.SetEnable(false);

        }

    }

    /// <summary>
    /// 離開遊戲
    /// </summary>
    private void OnQuitGame()
    {

        Application.Quit();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
#endif

    }

    /// <summary>
    /// 音量
    /// </summary>
    public void OnVolume()
    {

        volumeSliderUI.SetActive(!volumeSliderUI.activeSelf);

    }

    /// <summary>
    /// 繼續遊戲
    /// </summary>
    public void OnContinueGame()
    {

        input.CursorStateChange(true);

    }

    /// <summary>
    /// 重置角色
    /// </summary>
    public void OnReset()
    {

        organism.PlayerData.PlayerController.enabled = false;
        organism.PlayerData.PlayerCharacterController.enabled = false;

        input.CursorStateChange(true);
        organism.PlayerData.Player.transform.position = organism.PlayerData.PlayerController.spawnPos;

        organism.PlayerData.PlayerController.enabled = true;
        organism.PlayerData.PlayerCharacterController.enabled = true;

    }

    /// <summary>
    /// 復活
    /// </summary>
    public async void OnRespawn()
    {

        var playerController = organism.PlayerData.PlayerController;

        if (playerController.enabled) return;

        input.CursorStateChange(true);

        await playerController.IsAlive();

    }

    #endregion

    private bool CheckOrganismNull()
    {

        if (organism == null)
        {

            organism = GameManagerSingleton.Instance.Organism;

        }

        return organism == null;

    }

    /// <summary>
    /// 計算當前FPS和milisecond延遲
    /// </summary>
    private void CalculateFPSAndMsec()
    {

        if (Time.timeScale != 1) return;

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        Text_FPS.text = $"{fps:0} fps";
        Text_ms.text = $"{msec:0.0} ms";

    }

    /// <summary>
    /// 更新玩家血條
    /// </summary>
    private void PlayerHealthUpdate()
    {

        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, organism.PlayerData.PlayerHealth.GetHealthRatio(), 0.3f);

    }

    /// <summary>
    /// 更新武器UI
    /// </summary>
    private void WeaponUIUpdate()
    {

        var weaponManager = organism.PlayerData.PlayerWeaponManager;

        for (int i = 0; i < 3; i++)
        {
            if (weaponManager.GetWeaponAtSlotIndex(i) == null) continue;

            float value = weaponManager.GetWeaponAtSlotIndex(i).CurrentAmmoRatio;

            weaponUI[i].weaponEnergy.fillAmount = Mathf.Lerp(weaponUI[i].weaponEnergy.fillAmount, value, 0.2f);

            if (weaponManager.GetWeaponAtSlotIndex(i) == weaponManager.GetActiveWeapon())
            {
                weaponUI[i].weaponPocket.transform.localScale = new Vector3(1f, 1f, 1f);
                weaponUI[i].weaponPocket.color = Color.white;
                weaponUI[i].weaponIcon.color = Color.white;
                weaponUI[i].weaponEnergy.color = Color.white;
            }
            else
            {
                // 縮小pocket
                weaponUI[i].weaponPocket.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                weaponUI[i].weaponPocket.color = Color.gray;
                weaponUI[i].weaponIcon.color = Color.gray;
                weaponUI[i].weaponEnergy.color = Color.gray;
            }
        }

    }

    /// <summary>
    /// 音量UI
    /// </summary>
    private void VolumeUI(bool isMute)
    {

        btnVolumeUI.SetActive(!isMute);
        btnMuteUI.SetActive(isMute);

    }

    private async void AliveUI()
    {

        if (Cursor.lockState == CursorLockMode.Locked)
        {

            aliveUI.SetActive(false);
            if (!aliveUI.activeSelf)
            {
                Time.timeScale = 1;
            }

        }
        else if (aliveUI.activeSelf)
        {
            await DelayAndStopTimeAsync(2000);//延遲停止，讓死亡動畫可以播完
        }

    }

    private void PauseUI()
    {

        if (Cursor.lockState == CursorLockMode.Locked)
            pauseUI.SetActive(false);
        else if (!aliveUI.activeSelf)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }

    }

    /// <summary>
    /// 等待傳入值的秒數後，停止遊戲的時間
    /// </summary>
    /// <param name="delaytime">等待的秒數</param>
    private async Task DelayAndStopTimeAsync(int delaytime)
    {
        await Task.Delay(delaytime); // 等待?秒

        Time.timeScale = 0; // 停止時間
    }

    #endregion

}

[Serializable]
public class WeaponUIElements
{
    public Image weaponPocket;  // 武器的能量或彈藥UI底圖
    public Image weaponIcon;    // 武器Icon
    public Image weaponEnergy;  // 武器的能量或彈藥UI
}
