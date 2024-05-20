using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [Header("背景音樂")]
    [SerializeField] AudioClip[] audios;

    AudioSource audioSource;

    int preaudio = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource.clip == null)
        {
            audioSource.clip = audios[preaudio];
            audioSource.Play();
        }
    }

    // 背景音樂
    public void AudioSelectPotato()
    {
        if (audioSource.clip == audios[0])
        {
            preaudio = 0;
            return;
        }

        audioSource.clip = audios[0];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void AudioSelectTown()
    {
        if (audioSource.clip == audios[1])
        {
            preaudio = 1;
            return;
        }


        audioSource.clip = audios[1];
        audioSource.volume = 0.141f;
        audioSource.Play();
    }

    public void AudioSelectWind()
    {
        if (audioSource.clip == audios[2])
        {
            preaudio = 2;
            return;
        }

        audioSource.clip = audios[2];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void AudioSelectSad()
    {
        if (audioSource.clip == audios[3])
        {
            preaudio = 3;
            return;
        }

        audioSource.clip = audios[3];
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void AudioSelectGravy()
    {
        if (audioSource.clip == audios[3])
        {
            preaudio = 3;
            return;
        }

        audioSource.clip = audios[4];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void PreAudio()
    {
        if (audioSource.clip == audios[preaudio]) return;

        audioSource.clip = audios[preaudio];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

}
