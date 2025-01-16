using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{

    #region -- 資源參考區 --

    [Header("Text")]
    [SerializeField, Tooltip("FPS")] private Text Text_FPS;
    [SerializeField, Tooltip("ms")] private Text Text_ms;

    #endregion

    #region -- 變數參考區 --

    float deltaTime = 0.0f;

    #endregion

    #region -- 初始化/運作 --

    void Awake()
    {
        // 標記此物件不會在場景切換時被卸載
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

        if(Time.timeScale != 1) return;

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        Text_FPS.text = $"{fps:0} fps";
        Text_ms.text = $"{msec:0.0} ms";
    }

    #endregion

}
