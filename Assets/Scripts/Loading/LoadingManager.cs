using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    #region -- 戈方把σ跋 --

    [Header("单瓜"), Tooltip("ち传初春单礶")]
    [SerializeField] Image loadingImage;

    [Header("Btn")]
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Quit;

    [Header("GameObject")]
    [SerializeField] GameObject startGameUI;

    [Header("CanvasGroup")]
    [SerializeField] CanvasGroup canvasGroup_LoadingUI;

    #endregion

    #region -- 把计把σ跋 --

    #region -- 盽计 --

    private const int FIVE_THOUSAND_MILLISECONDS = 5000;

    #endregion

    InputController input;

    #endregion

    #region -- ﹍て/笲 --

    void Awake()
    {

        Init();

    }

    #endregion

    #region -- よ猭把σ跋 --

    /// <summary>
    /// ﹍て
    /// </summary>
    private void Init()
    {

        input = GameManagerSingleton.Instance.InputController;

        // 磷ち传初春瘆胊
        DontDestroyOnLoad(this.gameObject);

        #region -- btn --

        btn_Start.onClick.AddListener(() => _ = onStartGame());
        btn_Quit.onClick.AddListener(onQuitGame);

        #endregion

    }

    #region -- onClick --

    /// <summary>
    /// Button-Start 更眎瓜
    /// </summary>
    private async Task onStartGame()
    {
        // 更Game
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
    /// 瞒秨笴栏
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
