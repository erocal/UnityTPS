using System.Threading.Tasks;
using UnityEngine;

public class Init : MonoBehaviour
{

    #region -- �ܼưѦҰ� --

    public UISystem UISystem
    {
        get; private set;
    }

    #endregion

    #region -- ��l��/�B�@ --

    private void Awake()
    {
        
        _ = GameManagerSingleton.Instance.UISystem;

    }

    #endregion

    #region -- ��k�ѦҰ� --

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
