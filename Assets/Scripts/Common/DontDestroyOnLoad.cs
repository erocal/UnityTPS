using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    #region -- 初始化/運作 --

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

}
