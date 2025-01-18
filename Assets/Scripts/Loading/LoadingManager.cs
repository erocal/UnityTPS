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
    [SerializeField] CanvasGroup canvasGroup_StartUI;

    #endregion

    #region -- 跑计把σ跋 --

    #region -- 盽计 --

    private const int FIVE_THOUSAND_MILLISECONDS = 5000;

    #endregion

    ActionSystem actionSystem;
    InputController input;
    Organism organism;

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
    /// Button-Start 更眎瓜
    /// </summary>
    private async Task onStartGame()
    {
        // 更Game
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
