using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    [Header("等待圖"), Tooltip("切換場景時的等待畫面")]
    [SerializeField] Image loadingImage;

    #region -- 參數參考區 --

    InputController input;

    int sceneIndex = -1;

    #endregion

    void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;

        // 避免切換場景破壞
        DontDestroyOnLoad(input);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    private async void Update()
    {

        // 如果按下滑鼠左鍵，且在開始畫面，就加載Game
        if (input.GetClick() && SceneManager.GetActiveScene().buildIndex == 0)
        {
            input.CursorStateLocked();
            ShowLoadingImage(0);

            await AddrssableAsync.LoadSceneAsync("samplescene", LoadSceneMode.Single);

        }

        // 如果已在切換的場景，且Loading圖還在啟動
        if (SceneManager.GetActiveScene().buildIndex != sceneIndex && loadingImage.IsActive())
        {
            HideLoadingImage();
        }
    }

    #region -- 方法參考區 --

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

    private void HideLoadingImage()
    {
        if (loadingImage != null)
        {
            loadingImage.gameObject.SetActive(false);
        }
    }

    #endregion
}
