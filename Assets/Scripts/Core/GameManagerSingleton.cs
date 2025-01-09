using ToolBox.Pools;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

// singleton單例模式
// 可以確保生成對象只有一個實例存在
// 開發遊戲會希望某個類別只有一個實例化物件就可以使用
public class GameManagerSingleton
{
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

}
