using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera跟隨的目標")]
    [SerializeField] Transform target;

    [Header("水平軸靈敏度")]
    [SerializeField] float sensitivity_X = 2;
    [Header("垂直軸靈敏度")]
    [SerializeField] float sensitivity_Y = 2;
    [Header("滾輪靈敏度")]
    [SerializeField] float sensitivity_Z = 5;

    [Header("最小垂直角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Header("最大垂直角度")]
    [SerializeField] float maxVerticalAngle = 85;
    [Header("相機與目標的距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Header("最小相機與目標的距離")]
    [SerializeField] float minDistance = 2;
    [Header("最大相機與目標的距離")]
    [SerializeField] float maxDistance = 25;
    [Header("PlayerTarget")]
    [SerializeField] GameObject player;
    [Header("受傷時的特效")]
    [SerializeField] ParticleSystem beHitParticle;
    [Header("加速時的特效")]
    [SerializeField] ParticleSystem caplockParticle;

    [Header("Pause的UI")]
    [SerializeField] GameObject pauseUI;
    [Header("重生的UI")]
    [SerializeField] GameObject aliveUI;
    [Header("音量鍵的UI")]
    [SerializeField] GameObject volumeUI;
    [Header("靜音時的UI")]
    [SerializeField] GameObject muteUI;
    [Header("音量條的UI")]
    [SerializeField] GameObject volumeSliderUI;


    [Header("Pause的音效")]
    [SerializeField] AudioClip pauseSFX;

    [Header("Offset")]
    [SerializeField] Vector3 offset;
    [SerializeField] float offset_Y = 100f;

    #region -- 變數參考區 --

    InputController input;
    AudioSource audioSource;
    PlayerController playercontroller;

    float mouse_X = 0;
    float mouse_Y = 30;

    bool isChange;
    
    // 應該是看滑鼠是不是鎖住的狀態
    bool isLocked = false;

    #endregion

    private void Awake()
    {

        input = GameManagerSingleton.Instance.InputController;
        playercontroller = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        #region -- 訂閱 --

        player.GetComponent<Health>().onDamage += OnDamage;
        playercontroller.onCaplock += OnCaplock;
        player.GetComponent<Health>().onDie += OnDie;

        #endregion

    }

    private void Update()
    {
        CheckVolumeMute();
    }

    private void LateUpdate()
    {
        
        if ( Cursor.lockState == CursorLockMode.Locked )
        {
            pauseUI.SetActive(false);
            aliveUI.SetActive(false);

            if (!aliveUI.activeSelf)
            {
                Time.timeScale = 1;
            }
            
            mouse_X += input.GetMouseXAxis() * sensitivity_X;
            mouse_Y += input.GetMouseYAxis() * sensitivity_Y;

            // 限制垂直角度
            mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);

            // 固定相機在玩家身後
            transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);
            transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0) * new Vector3(0, 0, -cameraToTargetDistance) + target.position + Vector3.up * offset.y;

            // 滑鼠滾輪控制遠近
            cameraToTargetDistance += input.GetMouseScrollWheelAxis() * sensitivity_Z;
            cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, minDistance, maxDistance);

            isLocked = false;
        }
        else
        {

            isLocked = true;

            if(!aliveUI.activeSelf)
            {
                pauseUI.SetActive(true);
                DelayAndStopTimeAsync(0);
            }
            else
            {
                DelayAndStopTimeAsync(2000);//延遲停止，讓死亡動畫可以播完
            }

        }

        if (isLocked != isChange)
        {
            audioSource.PlayOneShot(pauseSFX);
            isChange = isLocked;
        }
    }

    #region -- 方法參考區 --

    /// <summary>
    /// 等待傳入值的秒數後，停止遊戲的時間
    /// </summary>
    /// <param name="delaytime">等待的秒數</param>
    private async Task DelayAndStopTimeAsync(int delaytime)
    {
        await Task.Delay(delaytime); // 等待?秒

        Time.timeScale = 0; // 停止時間
    }

    /// <summary>
    /// 確認目前音量是否為零，為零切換UI
    /// </summary>
    private void CheckVolumeMute()
    {
        if(audioSource.volume == 0)
        {
            volumeUI.SetActive(false);
            muteUI.SetActive(true);
        }
        else
        {
            volumeUI.SetActive(true);
            muteUI.SetActive(false);
        }
    }

    #endregion

    #region -- 事件相關 --

    /// <summary>
    /// 玩家死亡時處理方法
    /// </summary>
    private void OnDie()
    {
        aliveUI.SetActive(true);
        input.CursorStateUnlocked();
    }

    /// <summary>
    /// 玩家受傷時處理方法
    /// </summary>
    private async void OnDamage()
    {
        if (beHitParticle == null) return;
        beHitParticle.Play();

        //player受傷動畫
        playercontroller.animator.SetTrigger("IsDamage");
        await Task.Delay(2000);
    }

    /// <summary>
    /// 玩家加速時處理方法
    /// </summary>
    private void OnCaplock()
    {
        if (caplockParticle == null) return;
        caplockParticle.Play();
    }

    #endregion

    #region -- UI的OnClick()關聯 --

    /// <summary>
    /// 離開遊戲
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            EditorApplication.ExitPlaymode();
        }
#endif
    }

    /// <summary>
    /// 繼續遊戲
    /// </summary>
    public void ContinueGame()
    {
        input.CursorStateLocked();
    }

    /// <summary>
    /// 復活
    /// </summary>
    public void Respawn()
    {
        input.CursorStateLocked();

        playercontroller.IsAlive();
    }

    /// <summary>
    /// 音量
    /// </summary>
    public void Volume()
    {
        if(volumeSliderUI.activeSelf)
            volumeSliderUI.SetActive(false);
        else
            volumeSliderUI.SetActive(true);
    }

    #endregion

}
