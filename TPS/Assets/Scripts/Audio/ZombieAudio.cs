using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    #region -- 資源參考區 --

    [Space(5)]
    [Header("閒置的音效")]
    [SerializeField] AudioClip zombieIdleSFX;
    [Header("追趕的音效")]
    [SerializeField] AudioClip zombieFollowSFX;

    #endregion

    #region -- 變數參考區 --

    AudioSource audioSource;

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 播放Zombie閒置的音效
    /// </summary>
    /// <param name="zombie">傳入的物件，用來抓取聲音組件，此處應為Zombie</param>
    public void ZombieIdle(GameObject zombie)
    {
        audioSource = zombie.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(zombieIdleSFX); 
    }

    /// <summary>
    /// 播放Zombie追趕的音效
    /// </summary>
    /// <param name="zombie">傳入的物件，用來抓取聲音組件，此處應為Zombie</param>
    public void ZombieFollow(GameObject zombie)
    {
        audioSource = zombie.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(zombieFollowSFX);
    }

    #endregion
}
