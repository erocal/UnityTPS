using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("等待圖"), Tooltip("切換場景時的等待畫面")]
    [SerializeField] Image loadingImage;

    #endregion

    #region -- 參數參考區 --

    InputController input;

    int sceneIndex = -1;

    #endregion

    #region -- 初始化/運作 --

    void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;

        // 避免切換場景破壞
        DontDestroyOnLoad(input);
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {

        // 如果已在切換的場景，且Loading圖還在啟動
        if (SceneManager.GetActiveScene().buildIndex != sceneIndex && loadingImage.IsActive())
        {
            HideLoadingImage();
        }

    }

    #endregion

    #region -- 方法參考區 --

    private void HideLoadingImage()
    {
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
    }

    #region -- onClick --

    /// <summary>
    /// Button-Start 加載下一張地圖
    /// </summary>
    public async void onStartGame()
    {
        // 加載Game
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            input.CursorStateLocked();
            ShowLoadingImage(0);

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

        }
    }

    #endregion

    /// <summary>
    /// 在切換場景時，將等待畫面顯示出來
    /// </summary>
    /// <param name="sceneIndex">切換前的場景索引</param>
    public void ShowLoadingImage(int sceneIndex)
    {
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(true);

            // 將初始介面關閉
            GameObject.Find("UI").SetActive(false);
        }

        this.sceneIndex = sceneIndex;
    }

    #endregion
}
