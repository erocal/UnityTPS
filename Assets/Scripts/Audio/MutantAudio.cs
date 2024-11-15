using UnityEngine;

public class MutantAudio : MonoBehaviour
{

    #region -- 資源參考區 --

    [Space(5)]
    [Header("吼叫的音效")]
    [SerializeField] AudioClip mutantRoarSFX;
    [Header("攻擊的音效")]
    [SerializeField] AudioClip mutantAttackSFX;

    #endregion

    #region -- 變數參考區 --

    AudioSource audioSource;

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 播放Mutant吼叫的音效
    /// </summary>
    /// <param name="mutant">傳入的物件，用來抓取聲音組件，此處應為Boss:Mutant</param>
    public void MutantRoar(GameObject mutant)
    {
        audioSource = mutant.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(mutantRoarSFX);
    }

    /// <summary>
    /// 播放Mutant攻擊的音效
    /// </summary>
    /// <param name="mutant">傳入的物件，用來抓取聲音組件，此處應為Boss:Mutant</param>
    public void MutantAttack(GameObject mutant)
    {
        audioSource = mutant.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(mutantAttackSFX);
    }

    #endregion

}
