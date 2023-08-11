using System;
using System.Threading.Tasks;
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
    [SerializeField] ParticleSystem CaplockParticle;
    [Header("Pause的UI")]
    [SerializeField] GameObject pauseUI;
    [Header("重生的UI")]
    [SerializeField] GameObject aliveUI;

    [Header("Pause的音效")]
    [SerializeField] AudioClip pauseSFX;

    [Header("Offset")]
    [SerializeField] Vector3 offset;
    [SerializeField] float offset_Y = 100f;

    InputController input;
    AudioSource audioSource;
    PlayerController playercontroller;

    RaycastHit hit;
    Ray ray;

    float mouse_X = 0;
    float mouse_Y = 30;
    int preaudio = 0;
    /*[HideInInspector] public string info;
    
    private const float ultDistance = 1000;
    private int targetMask;*/

    bool isChange;
    
    // 應該是看滑鼠是不是鎖住的狀態
    bool isLocked = false;


    private void Awake()
    {
        /*info = null;
        targetMask = LayerMask.GetMask("Enemy");*/
        input = GameManagerSingleton.Instance.InputController;
        playercontroller = player.GetComponent<PlayerController>();
        player.GetComponent<Health>().onDamage += OnDamage;
        playercontroller.onCaplock += OnCaplock;
        player.GetComponent<Health>().onDie += OnDie;
        audioSource = GetComponent<AudioSource>();
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

    private async Task DelayAndStopTimeAsync(int delaytime)
    {
        await Task.Delay(delaytime); // 等待?秒

        Time.timeScale = 0; // 停止時間
    }

    private void OnDie()
    {
        aliveUI.SetActive(true);
        input.CursorStateUnlocked();
    }

    private async void OnDamage()
    {
        if (beHitParticle == null) return;
        beHitParticle.Play();

        //player受傷動畫
        playercontroller.animator.SetTrigger("IsDamage");
        await Task.Delay(2000);

    }

    private void OnCaplock()
    {
        if (CaplockParticle == null) return;
        CaplockParticle.Play();
    }

    #region -- UI的OnClick()關聯 --

    public void QuitGame()
    {
        Application.Quit();
        //EditorApplication.isPlaying = false;
    }

    public void ContinueGame()
    {
        input.CursorStateLocked();
    }

    public void Respawn()
    {
        input.CursorStateLocked();

        playercontroller.IsAlive();
    }

    #endregion

}
