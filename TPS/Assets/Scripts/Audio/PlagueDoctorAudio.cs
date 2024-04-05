using UnityEngine;

public class PlagueDoctorAudio : MonoBehaviour
{

    #region -- 資源參考區 --

    [Space(5)]
    [Header("火球的音效")]
    [SerializeField] AudioClip plagueDoctorFireballSFX;
    [Header("電擊的音效")]
    [SerializeField] AudioClip plagueDoctorLightingSFX;

    #endregion

    #region -- 變數參考區 --

    AudioSource audioSource;

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 播放PlagueDoctor火球的音效
    /// </summary>
    /// <param name="plagueDoctor">傳入的物件，用來抓取聲音組件，此處應為Boss:PlagueDoctor</param>
    public void PlagueDoctorFireBall(GameObject plagueDoctor)
    {
        audioSource = plagueDoctor.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(plagueDoctorFireballSFX);
    }

    /// <summary>
    /// 播放PlagueDoctor電擊的音效
    /// </summary>
    /// <param name="plagueDoctor">傳入的物件，用來抓取聲音組件，此處應為Boss:PlagueDoctor</param>
    public void PlagueDoctorLighting(GameObject plagueDoctor)
    {
        audioSource = plagueDoctor.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(plagueDoctorLightingSFX);
    }

    #endregion

}
