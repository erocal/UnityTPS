using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    #region -- 變數參考區 --

    #region -- 常數 --

    private const string SO_ADDRESS = "backgroundmusicso";

    #endregion

    private BackgroundMusicSO backgroundMusicSO;

    AudioSource audioSource;

    int preaudio = 0;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        Init();

        if (audioSource != null && backgroundMusicSO != null && audioSource.clip == null)
        {

            audioSource.clip = backgroundMusicSO.backgroundMusics[preaudio].BackgroundMusic;
            audioSource.Play();

        }

    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 初始化參數
    /// </summary>
    private async void Init()
    {

        if (backgroundMusicSO == null)
        {

            var handle = await AddrssableAsync.LoadAsync<BackgroundMusicSO>(SO_ADDRESS);

            if (handle.IsValid())
            {
                backgroundMusicSO = handle.Result;
            }

        }

        audioSource = GetComponent<AudioSource>();

    }

    // 背景音樂
    public void AudioSelectPotato()
    {
        if (audioSource.clip == backgroundMusicSO.backgroundMusics[0].BackgroundMusic)
        {
            preaudio = 0;
            return;
        }

        audioSource.clip = backgroundMusicSO.backgroundMusics[0].BackgroundMusic;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void AudioSelectTown()
    {
        if (audioSource.clip == backgroundMusicSO.backgroundMusics[1].BackgroundMusic)
        {
            preaudio = 1;
            return;
        }


        audioSource.clip = backgroundMusicSO.backgroundMusics[1].BackgroundMusic;
        audioSource.volume = 0.141f;
        audioSource.Play();
    }

    public void AudioSelectWind()
    {
        if (audioSource.clip == backgroundMusicSO.backgroundMusics[2].BackgroundMusic)
        {
            preaudio = 2;
            return;
        }

        audioSource.clip = backgroundMusicSO.backgroundMusics[2].BackgroundMusic;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void AudioSelectSad()
    {
        if (audioSource.clip == backgroundMusicSO.backgroundMusics[3].BackgroundMusic)
        {
            preaudio = 3;
            return;
        }

        audioSource.clip = backgroundMusicSO.backgroundMusics[3].BackgroundMusic;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void AudioSelectGravy()
    {
        if (audioSource.clip == backgroundMusicSO.backgroundMusics[3].BackgroundMusic)
        {
            preaudio = 3;
            return;
        }

        audioSource.clip = backgroundMusicSO.backgroundMusics[4].BackgroundMusic;
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    public void PreAudio()
    {

        var bgm = backgroundMusicSO.backgroundMusics[preaudio];

        if (audioSource.clip == bgm.BackgroundMusic) return;

        audioSource.clip = bgm.BackgroundMusic;
        audioSource.volume = bgm.Volume;
        audioSource.Play();

    }

    #endregion

}
