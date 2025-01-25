#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{

    #region -- �귽�ѦҰ� --

    [Header("GameObject")]
    [SerializeField, Tooltip("�ǬPIcon")] GameObject crosshair;

    [Header("Text")]
    [SerializeField, Tooltip("FPS")] private Text Text_FPS;
    [SerializeField, Tooltip("ms")] private Text Text_ms;

    [Header("���ݹ�"), Tooltip("���������ɪ����ݵe��")]
    [SerializeField] Image loadingImage;

    [Header("Btn")]
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Quit;
    [SerializeField] Button pauseUI_Btn_Quit;
    [SerializeField] Button btn_Volume;
    [SerializeField] Button btn_Mute;
    [SerializeField] Button btn_Continue;
    [SerializeField] Button btn_Respawn;
    [SerializeField] Button aliveUI_Btn_Respawn;

    [Header("Slider")]
    [SerializeField] Slider Slider_Music;

    [Header("GameObject")]
    [SerializeField, Tooltip("�C���}�lUI")] GameObject startGameUI;
    [SerializeField, Tooltip("Pause��UI")] GameObject pauseUI;
    [SerializeField, Tooltip("���ͪ�UI")] GameObject aliveUI;
    [SerializeField, Tooltip("���q�䪺UI")] GameObject btnVolumeUI;
    [SerializeField, Tooltip("�R���ɪ�UI")] GameObject btnMuteUI;
    [SerializeField, Tooltip("���q����UI")] GameObject volumeSliderUI;

    [Header("Image")]
    [SerializeField] Image healthImage;
    [SerializeField] Image Image_Gazed;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup canvasGroup_StartUI;

    #endregion

    #region -- �ܼưѦҰ� --

    private static UISystem _instance;

    public static UISystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<UISystem>();
            }

            return _instance;
        }
        private set { }
    }

    public GameObject CrossHair
    {
        get { return crosshair; }
    }

    #region -- �`�� --

    private const int FIVE_THOUSAND_MILLISECONDS = 5000;

    #endregion

    ActionSystem actionSystem;
    InputController input;
    Organism organism;

    private Health playerHealth;

    private Color32 originalImageGazedColor = new Color32(229, 23, 24, 168);

    private float deltaTime = 0.0f;

    #endregion

    #region -- ��l��/�B�@ --

    // ����~����ҤƸ���
    private UISystem()
    {
    }

    private void Awake()
    {

        GetInstance();

        Init();

    }

    private void Update()
    {

        CalculateFPSAndMsec();
        PlayerHealthUpdate();

    }

    private void LateUpdate()
    {

        AliveUI();
        PauseUI();

    }

    private void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    #region -- ��k�ѦҰ� --

    /// <summary>
    /// ����ߤ@���
    /// </summary>
    private void GetInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    /// <summary>
    /// ��l��
    /// </summary>
    private void Init()
    {

        actionSystem = GameManagerSingleton.Instance.ActionSystem;
        input = GameManagerSingleton.Instance.InputController;
        organism = Organism.Instance;

        playerHealth = organism.GetPlayer().GetComponent<Health>();

        actionSystem.OnDie += OnDie;
        actionSystem.OnCameraVolumeMute += VolumeUI;
        actionSystem.OnGazed += ImageGazedChangeColor;

        #region -- btn --

        btn_Start.onClick.AddListener(async () => await OnStartGame());
        btn_Quit.onClick.AddListener(OnQuitGame);
        pauseUI_Btn_Quit.onClick.AddListener(OnQuitGame);
        btn_Volume.onClick.AddListener(OnVolume);
        btn_Mute.onClick.AddListener(OnVolume);
        btn_Continue.onClick.AddListener(OnContinueGame);
        btn_Respawn.onClick.AddListener(OnRespawn);
        aliveUI_Btn_Respawn.onClick.AddListener(OnRespawn);

        #endregion

        Slider_Music.onValueChanged.AddListener(actionSystem.CameraVolumeChange);

    }

    private void ImageGazedChangeColor(bool inGazed)
    {

        Image_Gazed.color = inGazed ? Color.green : originalImageGazedColor;

    }

    /// <summary>
    /// ���a���`�ɳB�z��k
    /// </summary>
    private void OnDie(int id)
    {

        if (id != organism.GetPlayer().GetInstanceID()) return;

        aliveUI.SetActive(true);
        input.CursorStateChange(false);

    }

    #region -- onClick --

    /// <summary>
    /// Button-Start �[���U�@�i�a��
    /// </summary>
    private async Task OnStartGame()
    {
        // �[��Game
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            input.CursorStateChange(true);

            Destroy(startGameUI);

            organism.GetPlayer().SetActive(true);
            organism.GetPlayer().GetComponent<CharacterController>().enabled = false;

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

            await Task.Delay(FIVE_THOUSAND_MILLISECONDS);

            organism.GetPlayer().GetComponent<CharacterController>().enabled = true;

            actionSystem.SpawnPointUpdate(organism.GetPlayer().GetComponent<PlayerController>().spawnPos, MapAreaType.StartArea);

            canvasGroup_StartUI.SetEnable(false);

        }
    }

    /// <summary>
    /// ���}�C��
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
    /// ���q
    /// </summary>
    public void OnVolume()
    {

        volumeSliderUI.SetActive(!volumeSliderUI.activeSelf);

    }

    /// <summary>
    /// �~��C��
    /// </summary>
    public void OnContinueGame()
    {

        input.CursorStateChange(true);

    }

    /// <summary>
    /// �_��
    /// </summary>
    public async void OnRespawn()
    {

        input.CursorStateChange(true);

        await organism.GetPlayer().GetComponent<PlayerController>().IsAlive();

    }

    #endregion

    /// <summary>
    /// �p���eFPS�Mmilisecond����
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
    /// ��s���a���
    /// </summary>
    private void PlayerHealthUpdate()
    {

        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount, playerHealth.GetHealthRatio(), 0.3f);

    }

    /// <summary>
    /// ���qUI
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
            await DelayAndStopTimeAsync(2000);//���𰱤�A�����`�ʵe�i�H����
        }

    }

    private void PauseUI()
    {

        if (Cursor.lockState == CursorLockMode.Locked)
            pauseUI.SetActive(false);
        else if (!aliveUI.activeSelf)
            pauseUI.SetActive(true);

    }

    /// <summary>
    /// ���ݶǤJ�Ȫ���ƫ�A����C�����ɶ�
    /// </summary>
    /// <param name="delaytime">���ݪ����</param>
    private async Task DelayAndStopTimeAsync(int delaytime)
    {
        await Task.Delay(delaytime); // ����?��

        Time.timeScale = 0; // ����ɶ�
    }

    #endregion

}
