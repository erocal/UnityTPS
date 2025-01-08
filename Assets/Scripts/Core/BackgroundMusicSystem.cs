using UnityEngine;

public class BackgroundMusicSystem : MonoBehaviour
{

    #region -- 變數參考區 --

    #region -- 常數 --

    private const string SO_ADDRESS = "backgroundmusicso";

    #endregion

    private BackgroundMusicSO backgroundMusicSO;

    private AudioSource audioSource;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {

        Init();

    }

    private void Update()
    {
        
        if(audioSource == null) audioSource = Camera.main.GetComponent<AudioSource>();

        if (audioSource != null && backgroundMusicSO != null && audioSource.clip == null)
        {

            BGMSelect(BackgroundMusicSO.BackgroundMusicType.Potato);

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

        audioSource = Camera.main.GetComponent<AudioSource>();

    }

    /// <summary>
    /// 切換背景音樂
    /// </summary>
    public void BGMSelect(BackgroundMusicSO.BackgroundMusicType backgroundMusicType)
    {

        if(audioSource == null || backgroundMusicSO == null) return;

        int bgmType = (int)backgroundMusicType;
        audioSource.clip = backgroundMusicSO.backgroundMusics[bgmType].BackgroundMusic;
        audioSource.volume = backgroundMusicSO.backgroundMusics[bgmType].Volume;
        audioSource.Play();

    }

    // 背景音樂
    public void AudioSelectPotato()
    {

        audioSource.clip = backgroundMusicSO.backgroundMusics[0].BackgroundMusic;
        audioSource.volume = backgroundMusicSO.backgroundMusics[0].Volume;
        audioSource.Play();

    }

    public void AudioSelectTown()
    {

        audioSource.clip = backgroundMusicSO.backgroundMusics[1].BackgroundMusic;
        audioSource.volume = backgroundMusicSO.backgroundMusics[1].Volume;
        audioSource.Play();

    }

    public void AudioSelectWind()
    {

        audioSource.clip = backgroundMusicSO.backgroundMusics[2].BackgroundMusic;
        audioSource.volume = backgroundMusicSO.backgroundMusics[2].Volume;
        audioSource.Play();

    }

    public void AudioSelectSad()
    {

        audioSource.clip = backgroundMusicSO.backgroundMusics[3].BackgroundMusic;
        audioSource.volume = backgroundMusicSO.backgroundMusics[3].Volume;
        audioSource.Play();

    }

    public void AudioSelectGravy()
    {

        audioSource.clip = backgroundMusicSO.backgroundMusics[4].BackgroundMusic;
        audioSource.volume = backgroundMusicSO.backgroundMusics[4].Volume;
        audioSource.Play();

    }

    #endregion

}
