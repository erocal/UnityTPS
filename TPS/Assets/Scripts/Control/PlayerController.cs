﻿using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    [Tooltip("移動速度")]
    [SerializeField] float moveSpeed = 8;
    [Tooltip("Shift加速的倍數")]
    [Range(1, 3)]
    [SerializeField] float sprintSpeedModifier = 2;
    [Tooltip("蹲下時的減速倍數")]
    [Range(0, 1)]
    [SerializeField] float crouchedSpeedModifer = 0.5f;
    [Tooltip("旋轉速度")]
    [SerializeField] float rotateSpeed = 5f;
    [Tooltip("加速度百分比")]
    [SerializeField] float addSpeedRatio = 0.1f;

    [Space(20)]
    [Header("跳躍參數")]
    [Tooltip("跳躍時向上施加的力量")]
    [SerializeField] float jumpForce = 15;
    [Tooltip("在空中下施加的力量")]
    [SerializeField] float gravityDownForce = 50;
    [Tooltip("檢查與地面之間的距離")]
    [SerializeField] float distanceToGround = 0.1f;
    [Header("儲存腳的位置")]
    [SerializeField] Transform feet;

    [Space(20)]
    [Header("準星Icon")]
    public GameObject crosshair;

    [Space(20)]
    [Header("休息的音效")]
    [SerializeField] AudioClip feelsleepSFX;
    [Header("跑步喘氣的音效")]
    [SerializeField] AudioClip runtiredSFX;
    [Header("跳躍時的音效")]
    [SerializeField] AudioClip jumptwiceSFX;
    //[SerializeField] AudioClip jumpSFX;
    [Header("瞄準時的音效")]
    [SerializeField] AudioClip targetlockonSFX;
    [Header("走路的音效")]
    [SerializeField] AudioClip stepSFX;
    [Header("跑步的音效")]
    [SerializeField] AudioClip runstepSFX;

    // 這是啟動瞄準的事件
    public event Action<bool> onAim;

    // 這是跑步特效的事件
    public event Action onCaplock;

    InputController inputController, input;
    CharacterController controller;
    [HideInInspector] public Animator animator;
    Health health;
    WeaponManager weaponManager;
    AudioSource audioSource;

    int jumpCount = 1;

    // 計時器
    private float resttimerrate = 2.0f;
    private float runtimerrate = 2.0f;
    private float steptimerrate = 2.0f;
    private float runsteptimerrate = 2.0f;
    //private float targetlockontimerrate = 2.0f;

    /// <summary>
    /// 出生點
    /// </summary>
    Vector3 spawn;
    // 下一幀要移動到的目標位置
    Vector3 targetMovement;
    // 下一幀跳躍到的方向
    Vector3 jumpDirection;
    // 復活點
    //Vector3 RespawnPosition;
    // 上一幀的移動速度
    float lastFrameSpeed = 0.0f;
    // 是否在瞄準狀態
    bool isAim;

    /*Vector3 cameraForward;
    Vector3 cameraRight;*/



    void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        weaponManager = GetComponent<WeaponManager>();
        audioSource = GetComponent<AudioSource>();
        //targetMask = LayerMask.GetMask("Terrain");

        spawn = new Vector3(-473.3f, 21.93f, 245.9f);

        // 訂閱死亡事件
        health.onDie += OnDie;
    }

    // Update is called once per frame
    private void Update()
    {
        //IsGrounded = Physics2D.OverlapCircle(feet.position,0.1f,Terrain);
        //print(inputController.horizontal + "Horizontal");
        //print(inputController.vertical + "Vertical");
        /*if (m_Input.GetMoveInput()!= Vector3.zero)
        {
            print(m_Input.GetMoveInput());
        }*/

        UpdateTimer();

        // 瞄準行為
        AimBehaviour();
        // 移動行為
        MoveBehaviour();
        // 跳躍行為
        JumpBehaviour();
        // 休息行為
        RestBehaviour();

    }

    #region -- 方法參考區 --

    /// <summary>
    /// 處理瞄準行為
    /// </summary>
    private void AimBehaviour()
    {
        bool lastTimeAim = isAim;
        if (input.GetFireInputDown() && weaponManager.GetActiveWeapon() != null)
        {
            isAim = true;
        }
        if (input.GetAimInputDown() && weaponManager.GetActiveWeapon() != null)
        {
            isAim = !isAim;
        }

        if (lastTimeAim != isAim)
        {
            if (crosshair != null)
            {
                crosshair.SetActive(isAim);
                if (targetlockonSFX != null /*&& targetlockontimerrate <= 0.0f*/ && crosshair.activeInHierarchy != false)
                {
                    audioSource.PlayOneShot(targetlockonSFX);
                    //targetlockontimerrate = 2.0f;
                }
            }
            onAim?.Invoke(isAim);
        }

        animator.SetBool("IsAim", isAim);
    }

    /// <summary>
    /// 處理移動行為
    /// </summary>
    private async void MoveBehaviour()
    {
        targetMovement = Vector3.zero;
        Vector3 pretargetMovement = targetMovement;
        targetMovement += input.GetMoveInput().z * GetCurrentCameraForward();
        targetMovement += input.GetMoveInput().x * GetCurrentCameraRight();

        if (targetMovement != pretargetMovement)
        {
            resttimerrate = 2.0f;
        }

        // 避免對角線超過1
        targetMovement = Vector3.ClampMagnitude(targetMovement, 1);

        // 下一幀的移動速度
        float nextFrameSpeed = 0;

        // 是否按下加速
        if (targetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0f;
        }

        // 如果加速鍵被按下且不在瞄準時
        else if (input.GetCapInput() && !isAim)
        {
            nextFrameSpeed = 1f;
            targetMovement *= sprintSpeedModifier;
            SmoothRotation(targetMovement);
            onCaplock?.Invoke();

            // 如果按下跳躍鍵，且人物處在地面上時
            if (input.GetJumpInputDown() && IsGrounded())
            {
                animator.SetTrigger("IsJump");
                jumpDirection = Vector3.zero;
                jumpDirection += jumpForce * Vector3.up;
                jumpCount = 0;
            }
        }

        /*else if (input.GetSprintInput() && !isAim)
        {
            print("2");
            nextFrameSpeed = 1f;
            targetMovement *= sprintSpeedModifier;
            SmoothRotation(targetMovement);
        }*/

        // 如果不處於瞄準
        else if (!isAim)
        {
            nextFrameSpeed = 0.5f;
            SmoothRotation(targetMovement);
        }

        // 處於瞄準時
        if (isAim)
        {
            SmoothRotation(GetCurrentCameraForward());
        }

        // 當前後Frame速度不一致，線性更改速度
        if (lastFrameSpeed != nextFrameSpeed)
        {
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);
        }

        // 當在走路時，撥放走路音效
        if (lastFrameSpeed > 0.2f && lastFrameSpeed <= 0.5f)
        {
            if (stepSFX != null && steptimerrate <= 1.2f)
            {
                audioSource.PlayOneShot(stepSFX);
                steptimerrate = 2.0f;
            }
        }

        // 當在奔跑時，撥放走路音效
        if (lastFrameSpeed > 0.7f)
        {
            if (runstepSFX != null && runsteptimerrate <= 1.7f)
            {
                audioSource.PlayOneShot(runstepSFX);
                runsteptimerrate = 2.0f;
            }
        }

        // 如果長時間奔跑，播放疲累音效
        if (lastFrameSpeed > 0.9f)
        {
            if (runtiredSFX != null && runtimerrate <= -0.426f)
            {
                audioSource.PlayOneShot(runtiredSFX);
                runtimerrate = 2.0f;
            }
        }

        // 更動unity裡面人物動畫的相關數值
        animator.SetFloat("WalkSpeed", lastFrameSpeed);
        animator.SetFloat("Vertical", input.GetMoveInput().z);
        animator.SetFloat("Horizontal", input.GetMoveInput().x);

        // 動態變化移動速度
        controller.Move(targetMovement * Time.deltaTime * moveSpeed);
    }
    
    /// <summary>
    /// 處理跳躍行為
    /// </summary>
    private void JumpBehaviour()
    {
        // 如果人物處於地面
        if (IsGrounded())
        {
            jumpCount = 1;
        }

        // 當人物處於地面，按下跳躍鍵且沒有瞄準時
        if (input.GetJumpInputDown() && jumpCount > 0 && !isAim && IsGrounded())
        {
            animator.SetTrigger("IsJump");
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;
            jumpCount = 0;
        }
        else if (input.GetJumpInputDown() && jumpCount == 0)
        {
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;
            jumpCount--;

            // 播放跳躍音效
            if (jumptwiceSFX != null)
            {
                audioSource.PlayOneShot(jumptwiceSFX);
            }
        }

        jumpDirection.y -= gravityDownForce * Time.deltaTime;
        jumpDirection.y = Mathf.Max(jumpDirection.y, -gravityDownForce);

        controller.Move(jumpDirection * Time.deltaTime);
    }

    /// <summary>
    /// 處理休息行為
    /// </summary>
    private void RestBehaviour()
    {
        if(resttimerrate <= -30.0f)
        {
            animator.SetTrigger("IsRest");
            resttimerrate = 2.0f;
            if (feelsleepSFX != null)
            {
                audioSource.PlayOneShot(feelsleepSFX);
            }
        }
    }

    /// <summary>
    /// 檢測玩家是否在地上
    /// </summary>
    /// <returns>回傳玩家是否在地上</returns>
    private bool IsGrounded()
    {
        //Debug.DrawRay(transform.position, -Vector3.up * distanceToGround, Color.yellow);
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround/*, targetMask*/);
    }

    /// <summary>
    /// 平滑旋轉角度到目標方向
    /// </summary>
    /// <param name="targetMovement">目標方向</param>
    private void SmoothRotation(Vector3 targetMovement)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMovement, Vector3.up), rotateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 取得目前相機的正面方向
    /// </summary>
    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f;
        //歸一化
        cameraForward.Normalize();
        return cameraForward;
    }

    /// <summary>
    /// 取得目前相機的右側方向
    /// </summary>
    private Vector3 GetCurrentCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;
        //歸一化
        cameraRight.Normalize();
        return cameraRight;
    }

    /// <summary>
    /// 將玩家改變位置
    /// </summary>
    private void ChangePosition(Vector3 teleportPosition)
    {
        this.transform.position = teleportPosition;
    }

    #region -- 事件相關 --

    /// <summary>
    /// 玩家死亡時處理方法
    /// </summary>
    private void OnDie()
    {
        animator.SetTrigger("IsDead");
        //取消玩家的控制
        this.GetComponent<PlayerController>().enabled = false;
    }

    #endregion

    /// <summary>
    /// 玩家復活
    /// </summary>
    public void IsAlive()
    {
        health.Alive();
        animator.SetTrigger("IsAlive");
        // 初始玩家生成位置
        ChangePosition(spawn);
        //還給玩家控制權
        this.GetComponent<PlayerController>().enabled = true;
    }

    /// <summary>
    /// 計時器
    /// </summary>
    private void UpdateTimer()
    {
        resttimerrate -= Time.deltaTime;
        runtimerrate -= Time.deltaTime;
        steptimerrate -= Time.deltaTime;
        runsteptimerrate -= Time.deltaTime;
    }

    #endregion

}
