using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    #region -- �귽�ѦҰ� --

    [Header("���ݹ�"), Tooltip("���������ɪ����ݵe��")]
    [SerializeField] Image loadingImage;

    [Header("Btn")]
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Quit;

    [Header("GameObject")]
    [SerializeField] GameObject startGameUI;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup canvasGroup_LoadingUI;

    #endregion

    #region -- �ѼưѦҰ� --

    #region -- �`�� --

    private const int FIVE_THOUSAND_MILLISECONDS = 5000;

    #endregion

    InputController input;

    #endregion

    #region -- ��l��/�B�@ --

    void Awake()
    {

        Init();

    }

    #endregion

    #region -- ��k�ѦҰ� --

    /// <summary>
    /// ��l��
    /// </summary>
    private void Init()
    {

        input = GameManagerSingleton.Instance.InputController;

        // �קK���������}�a
        DontDestroyOnLoad(this.gameObject);

        #region -- btn --

        btn_Start.onClick.AddListener(() => _ = onStartGame());
        btn_Quit.onClick.AddListener(onQuitGame);

        #endregion

    }

    #region -- onClick --

    /// <summary>
    /// Button-Start �[���U�@�i�a��
    /// </summary>
    private async Task onStartGame()
    {
        // �[��Game
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            input.CursorStateChange(true);

            Destroy(startGameUI);

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

            await Task.Delay(FIVE_THOUSAND_MILLISECONDS);

            canvasGroup_LoadingUI.SetEnable(false);

        }
    }

    /// <summary>
    /// ���}�C��
    /// </summary>
    private void onQuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
#endif
    }

    #endregion

    #endregion

}
