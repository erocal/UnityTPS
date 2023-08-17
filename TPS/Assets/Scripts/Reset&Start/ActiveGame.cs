using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveGame : MonoBehaviour
{
    #region -- 參數參考區 --

    InputController input;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;

        // 確保 GameManagerSingleton 的實例不被銷毀
        DontDestroyOnLoad(input);
    }

    void Update()
    {
        // 如果按下滑鼠左鍵，就加載Game
        if (input.GetClick())
        {
            input.CursorStateLocked();
            SceneManager.LoadScene(0);
        }
    }

    #endregion 
}
