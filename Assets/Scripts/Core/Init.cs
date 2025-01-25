using System.Threading.Tasks;
using UnityEngine;

public class Init : MonoBehaviour
{

    #region -- 變數參考區 --

    public UISystem UISystem
    {
        get; private set;
    }

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        
        _ = GameManagerSingleton.Instance.UISystem;

    }

    #endregion

    #region -- 方法參考區 --

    public async Task CreateHUDManagerAsync()
    {

        try
        {
            var obj = await AddrssableAsync.LoadInstantiate("hudmanager");
            if (obj != null)
            {
                UISystem = obj.GetComponent<UISystem>();
            }
        }
        catch (System.Exception e)
        {
            Log.Error(e);
        }

    }

    #endregion

}
