using UnityEngine;

public class PlagueDoctorAudio : MonoBehaviour
{
    [Space(5)]
    [Header("火球的音效")]
    [SerializeField] AudioClip plaguedoctorfireballSFX;
    [Header("電擊的音效")]
    [SerializeField] AudioClip plaguedoctorlightingSFX;

    AudioSource audioSource;

    public void plaguedoctorfireball(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();
        if (plaguedoctorfireballSFX != null)
        {
            audioSource.PlayOneShot(plaguedoctorfireballSFX);
        }

    }

    public void plaguedoctorlightingball(GameObject other)
    {
        audioSource = other.GetComponent<AudioSource>();
        if (plaguedoctorlightingSFX != null)
        {
            audioSource.PlayOneShot(plaguedoctorlightingSFX);
        }

    }
}
