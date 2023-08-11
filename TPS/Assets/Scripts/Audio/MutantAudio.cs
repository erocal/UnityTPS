using UnityEngine;

public class MutantAudio : MonoBehaviour
{
    [Space(5)]
    [Header("吼叫的音效")]
    [SerializeField] AudioClip mutantRoarSFX;
    [Header("攻擊的音效")]
    [SerializeField] AudioClip mutantAttackSFX;

    AudioSource audioSource;

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
