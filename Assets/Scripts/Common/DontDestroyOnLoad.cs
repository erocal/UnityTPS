using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{

    #region -- ��l��/�B�@ --

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

}
