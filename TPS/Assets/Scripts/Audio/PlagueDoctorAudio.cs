using UnityEngine;

public class PlagueDoctorAudio : MonoBehaviour
{
    #region -- 物件參考區 --

    [Space(5)]
    [Header("火球的音效")]
    [SerializeField] AudioClip plaguedoctorfireballSFX;
    [Header("電擊的音效")]
    [SerializeField] AudioClip plaguedoctorlightingSFX;

    #endregion

    #region -- 變數參考區 --

    AudioSource audioSource;

    #endregion

    #region -- 方法 --

    /// <summary>
    /// 播放PlagueDoctor火球的音效
    /// </summary>
    /// <param name="other">傳入的物件，用來抓取聲音組件，此處應為Boss:PlagueDoctor</param>
    public void PlagueDoctorFireBall(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(plaguedoctorfireballSFX);
    }

    /// <summary>
    /// 播放PlagueDoctor電擊的音效
    /// </summary>
    /// <param name="other">傳入的物件，用來抓取聲音組件，此處應為Boss:PlagueDoctor</param>
    public void PlagueDoctorLighting(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(plaguedoctorlightingSFX);
    }

    #endregion
}
