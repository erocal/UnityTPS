using UnityEngine;

public class Organism : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField, Tooltip("玩家")] private GameObject player;
    [SerializeField, Tooltip("暴徒")] private GameObject mutant;
    [SerializeField, Tooltip("瘟疫醫生")] private GameObject plagueDoctor;

    #endregion

    #region -- 變數參考區 --

    private static Organism _instance;

    public static Organism Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindFirstObjectByType<Organism>();
            }

            return _instance;
        }
        private set { }
    }

    #endregion

    #region -- 初始化/運作 --

    // 防止外部實例化該類
    private Organism()
    {
    }

    private void Awake()
    {
        GetInstance();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    #endregion

    #region -- 方法參考區 --

    #region -- 單例模式 --

    /// <summary>
    /// 獲取唯一實例
    /// </summary>
    private void GetInstance()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    #endregion

    #region -- Get方法 --

    /// <summary>
    /// 取得玩家
    /// </summary>
    public GameObject GetPlayer()
    {
        if (player == null) Log.Error("玩家物件為空");

        return player;
    }

    /// <summary>
    /// 取得暴徒Boss
    /// </summary>
    public GameObject GetMutant()
    {
        if (mutant == null) Log.Error("暴徒Boss物件為空");

        return mutant;
    }

    /// <summary>
    /// 取得瘟疫醫生Boss
    /// </summary>
    public GameObject GetPlagueDoctor()
    {
        if (plagueDoctor == null) Log.Error("瘟疫醫生Boss物件為空");

        return plagueDoctor;
    }

    #endregion

    #endregion

}
