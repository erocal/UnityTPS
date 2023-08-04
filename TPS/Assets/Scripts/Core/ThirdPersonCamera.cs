using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;

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

    [Header("Pause的音效")]
    [SerializeField] AudioClip pauseSFX;

    [Header("背景音樂")]
    [SerializeField] AudioClip[] audios;

    [Header("Offset")]
    [SerializeField] Vector3 offset;
    [SerializeField] float offset_Y = 100f;

    [Header("等待秒數")]
    [SerializeField] float playerondamagerate = 2.0f;

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
    bool isLocked = false;


    private void Awake()
    {
        /*info = null;
        targetMask = LayerMask.GetMask("Enemy");*/
        input = GameManagerSingleton.Instance.InputController;
        player.GetComponent<Health>().onDamage += OnDamage;
        player.GetComponent<PlayerController>().onCaplock += OnCaplock;
        audioSource = GetComponent<AudioSource>();
        playercontroller = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        playerondamagerate -= Time.deltaTime;
        if (audioSource.clip == null)
        {
            audioSource.clip = audios[preaudio];
            audioSource.Play();
        }
    }

    private void LateUpdate()
    {
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;

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
            pauseUI.SetActive(true);
            Time.timeScale = 0;
            isLocked = true;
        }

        if (isLocked != isChange)
        {
            audioSource.PlayOneShot(pauseSFX);
            isChange = isLocked;
        }
    }

    private void OnDamage()
    {
        if (beHitParticle == null) return;
        beHitParticle.Play();

        if (playerondamagerate < 0.0f)
        {
            //player受傷動畫
            playercontroller.animator.SetTrigger("IsDamage");
            playerondamagerate = 2.0f;
        }

    }

    private void OnCaplock()
    {
        if (CaplockParticle == null) return;
        CaplockParticle.Play();
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

    public void QuitGame()
    {
        Application.Quit();
        //EditorApplication.isPlaying = false;
    }

    public void ContinueGame()
    {
        input.CursorStateLocked();
    }

}
