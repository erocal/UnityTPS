using System.Threading.Tasks;
using ToolBox.Pools;
using UnityEngine;
using UnityEngine.Localization.PropertyVariants;

// singleton單例模式
// 可以確保生成對象只有一個實例存在
// 開發遊戲會希望某個類別只有一個實例化物件就可以使用
public class GameManagerSingleton
{

    private bool m_IsUISystemInitializing = false;

    private GameObject gameObject;

    //單例
    private static GameManagerSingleton m_Instance;
    //接口，確認單例是否存在
    public static GameManagerSingleton Instance
    {
        get
        {
            if (m_Instance == null)
            {

                m_Instance = new GameManagerSingleton
                {
                    gameObject = new GameObject("Core"),
                };

                m_Instance.gameObject.AddComponent<Init>();
                m_Instance.gameObject.AddComponent<DontDestroyOnLoad>();
                m_Instance.gameObject.AddComponent<InputController>();
                m_Instance.gameObject.AddComponent<PoolInstaller>();
                m_Instance.gameObject.AddComponent<BackgroundMusicSystem>();

            }
            return m_Instance;
        }
    }

    // 登記InputController(一個遊戲只會有一個)
    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
            {
                m_InputController = gameObject.GetComponent<InputController>();
            }
            return m_InputController;
        }
    }

    private PoolInstaller m_PoolInstaller;
    public PoolInstaller PoolInstaller
    {
        get
        {
            if (m_PoolInstaller == null)
            {
                m_PoolInstaller = gameObject.GetComponent<PoolInstaller>();
            }
            return m_PoolInstaller;
        }
    }

    private BackgroundMusicSystem m_BackgroundMusicSystem;
    public BackgroundMusicSystem BackgroundMusicSystem
    {
        get
        {
            if (m_BackgroundMusicSystem == null)
            {
                m_BackgroundMusicSystem = gameObject.GetComponent<BackgroundMusicSystem>();
            }
            return m_BackgroundMusicSystem;
        }
    }

    private ActionSystem m_ActionSystem;
    public ActionSystem ActionSystem
    {
        get
        {
            m_ActionSystem ??= new ActionSystem();
            return m_ActionSystem;
        }
    }

    private UISystem m_UISystem;
    public UISystem UISystem
    {
        get
        {

            // 如果已經初始化，直接返回
            if (m_UISystem != null)
            {
                return m_UISystem;
            }

            // 避免重複初始化
            if (m_IsUISystemInitializing)
            {
                return null; // 或者可以拋出異常，提示正在初始化
            }

            m_IsUISystemInitializing = true;

            var init = gameObject.GetComponent<Init>();

            // 如果 Init 組件中已經有 UISystem，直接使用
            if (init.UISystem != null)
            {
                m_UISystem = init.UISystem;
                m_IsUISystemInitializing = false;
                return m_UISystem;
            }

            // 非同步初始化 UISystem
            init.CreateHUDManagerAsync().ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    m_UISystem = init.UISystem;
                }
                else
                {
                    Log.Error("Failed to initialize UISystem: " + task.Exception);
                }
                m_IsUISystemInitializing = false;
            });

            return null;

        }
    }

    // 實現IDisposable接口
    public void Dispose()
    {

        // 銷毀GameObject及其所有組件
        if (gameObject != null)
        {
            Object.Destroy(gameObject);
            gameObject = null;
        }

        // 釋放其他資源（如果有的話）
        m_ActionSystem = null;

        // 清除單例實例
        m_Instance = null;

    }

    // 析構函數，確保資源被釋放
    ~GameManagerSingleton()
    {
        Dispose();
    }

}
