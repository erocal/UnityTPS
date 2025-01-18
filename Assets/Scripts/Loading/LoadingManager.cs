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
    [SerializeField] CanvasGroup canvasGroup_StartUI;

    #endregion

    #region -- �ܼưѦҰ� --

    #region -- �`�� --

    private const int FIVE_THOUSAND_MILLISECONDS = 5000;

    #endregion

    ActionSystem actionSystem;
    InputController input;
    Organism organism;

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

        actionSystem = GameManagerSingleton.Instance.ActionSystem;
        input = GameManagerSingleton.Instance.InputController;
        organism = Organism.Instance;

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

            organism.GetPlayer().SetActive(true);

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

            await Task.Delay(FIVE_THOUSAND_MILLISECONDS);

            actionSystem.SpawnPointUpdate(organism.GetPlayer().GetComponent<PlayerController>().spawnPos, MapAreaType.StartArea);

            canvasGroup_StartUI.SetEnable(false);

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
