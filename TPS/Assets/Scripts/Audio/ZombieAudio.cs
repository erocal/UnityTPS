using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    #region -- 物件參考區 --

    [Space(5)]
    [Header("閒置的音效")]
    [SerializeField] AudioClip zombieidleSFX;
    [Header("追趕的音效")]
    [SerializeField] AudioClip zombiefollowSFX;

    #endregion

    #region -- 變數參考區 --

    AudioSource audioSource;

    #endregion

    /// <summary>
    /// 播放Zombie閒置的音效
    /// </summary>
    /// <param name="other">傳入的物件，用來抓取聲音組件，此處應為Boss:Zombie</param>
    public void ZombieIdle(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(zombieidleSFX); 
    }

    /// <summary>
    /// 播放Zombie追趕的音效
    /// </summary>
    /// <param name="other">傳入的物件，用來抓取聲音組件，此處應為Boss:Zombie</param>
    public void ZombieFollow(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();

        audioSource?.PlayOneShot(zombiefollowSFX);
    }
}
