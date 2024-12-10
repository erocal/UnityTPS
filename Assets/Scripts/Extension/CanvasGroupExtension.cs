using UnityEngine;
using DG.Tweening;

public static class CanvasGroupExtension
{
    public static bool IsEnable(this CanvasGroup canvasGroup)
    {
        return (canvasGroup.alpha > 0.0001f)
            && canvasGroup.interactable
            && canvasGroup.blocksRaycasts;
    }

    public static void SetEnable(this CanvasGroup canvasGroup, bool isEnable)
    {
        if (isEnable)
        {
            if (!canvasGroup.gameObject.activeSelf)
                canvasGroup.gameObject.SetActive(true);

            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    public static void SetEnableForDG(this CanvasGroup canvasGroup, bool isEnale)
    {
        if (!isEnale)
        {
            if (!canvasGroup.gameObject.activeSelf)
                canvasGroup.gameObject.SetActive(true);
        }
        canvasGroup.interactable = isEnale;
        canvasGroup.blocksRaycasts = isEnale;
    }


    public static void FadeIn(this CanvasGroup canvasGroup, float duration = 0.4f)
    {
        if (!canvasGroup.gameObject.activeSelf)
            canvasGroup.gameObject.SetActive(true);

        if (canvasGroup.interactable == true)
            return;

        if (duration <= 0)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.DOFade(1, duration).OnComplete(() => { canvasGroup.interactable = true; canvasGroup.blocksRaycasts = true; });
        }
    }

    public static void FadeOut(this CanvasGroup canvasGroup, float duration = 0.4f)
    {
        if (canvasGroup.interactable == false)
            return;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        if (duration <= 0)
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            canvasGroup.DOFade(0, duration);
        }
    }
}
