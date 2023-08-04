using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantAudio : MonoBehaviour
{
    [Space(5)]
    [Header("吼叫的音效")]
    [SerializeField] AudioClip mutantRoarSFX;
    [Header("攻擊的音效")]
    [SerializeField] AudioClip mutantAttackSFX;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void mutantroar(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();
        if (mutantRoarSFX != null)
        {
            audioSource.PlayOneShot(mutantRoarSFX);
        }

    }

    public void mutantattack(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();
        if (mutantAttackSFX != null)
        {
            audioSource.PlayOneShot(mutantAttackSFX);
        }

    }
}
