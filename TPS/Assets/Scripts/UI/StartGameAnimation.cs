using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameAnimation : MonoBehaviour
{
    // 離散的變換文字大小，因為時間暫停了

    [SerializeField] int changeFontSizeAmount = 20; // Amount to scale up or down
    [SerializeField] float interval = 1.0f;    // Time interval for scaling change
    
    private Text originalFontSize;
    private bool isScalingUp = true;

    private void Awake()
    {
        originalFontSize = this.GetComponent<Text>();
    }

    private void Start()
    {
        StartCoroutine(ChangeFontSize());
    }

    private IEnumerator ChangeFontSize()
    {
        while (true)
        {
            if (isScalingUp)
            {
                this.GetComponent<Text>().fontSize = originalFontSize.fontSize + changeFontSizeAmount;
            }
            else
            {
                this.GetComponent<Text>().fontSize = originalFontSize.fontSize - changeFontSizeAmount;
            }

            isScalingUp = !isScalingUp;

            yield return new WaitForSecondsRealtime(interval);
        }
    }
}
