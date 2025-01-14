using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAnimation : MonoBehaviour
{

    #region -- 資源參考區 --

    [SerializeField] float scaleRange = 20.0f;   // 縮放範圍
    [SerializeField] float baseFontSize = 40.0f; // 字體大小

    [SerializeField] float animationDuration = 0.5f; // 動畫持續時間

    #endregion

    #region -- 變數參考區 --

    private Text text;
    private float animationStartTime;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        var myScale = 1;
        Time.timeScale = myScale;
        text = GetComponent<Text>();
    }

    private void Start()
    {
        animationStartTime = Time.time;
    }

    private void FixedUpdate()
    {
        if(text.fontSize == 40.0f)
        {
            StartCoroutine(ScaleText());
        }
    }

    #endregion

    #region -- 方法參考區 --

    private IEnumerator ScaleText()
    {
        while (true)
        {
            float elapsedTime = Time.time - animationStartTime;
            float t = Mathf.PingPong(elapsedTime / animationDuration, 1.0f);

            // 平滑
            float easedT = Mathf.SmoothStep(0.0f, 1.0f, t);

            float fontSize = baseFontSize + Mathf.Lerp(0, scaleRange, easedT);
            text.fontSize = (int)fontSize;

            yield return null;
        }
    }

    #endregion

}
