using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAnimation : MonoBehaviour
{
    [SerializeField] float scaleRange = 20.0f;   // 字体大小缩放范围
    [SerializeField] float baseFontSize = 40.0f; // 基础字体大小

    [SerializeField] float animationDuration = 0.5f; // 动画持续时间

    private Text text;
    private float animationStartTime;


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

    private IEnumerator ScaleText()
    {
        while (true)
        {
            float elapsedTime = Time.time - animationStartTime;
            float t = Mathf.PingPong(elapsedTime / animationDuration, 1.0f);

            // 使用缓动函数 SmoothStep 来平滑变化
            float easedT = Mathf.SmoothStep(0.0f, 1.0f, t);

            float fontSize = baseFontSize + Mathf.Lerp(0, scaleRange, easedT);
            text.fontSize = (int)fontSize;

            yield return null;
        }
    }
}
