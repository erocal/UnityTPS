using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    [Space(5)]
    [Header("閒置的音效")]
    [SerializeField] AudioClip zombieidleSFX;
    [Header("追趕的音效")]
    [SerializeField] AudioClip zombiefollowSFX;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void zombieidle(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();
        if(zombieidleSFX != null)
        {
            audioSource.PlayOneShot(zombieidleSFX);
        }
            
    }

    public void zombiefollow(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();
        if (zombiefollowSFX != null)
        {
            audioSource.PlayOneShot(zombiefollowSFX);
        }

    }
}
