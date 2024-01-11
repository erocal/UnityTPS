using UnityEngine;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    #region -- 參數參考區 --

    public float vertical;
    public float horizontal;

    #endregion

    #region -- 初始化/運作 --

    private void Awake()
    {
        //設定游標狀態 (鎖定)
        Cursor.lockState = CursorLockMode.None;
        // 是否顯示游標
        Cursor.visible = true;
    }

    private void Update()
    {
        CheckCursorState();
        //vertical = Input.GetAxis("Vertical");
        //horizontal = Input.GetAxis("Horizontal");
    }

    #endregion

    #region -- 方法參考區 --

    /// <summary>
    /// 取得WASD的Axis
    /// </summary>
    public Vector3 GetMoveInput()
    {
        if(CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //限制速度
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }
        
        return Vector3.zero;
    }

    /// <summary>
    /// 是否按下CapsLock加速
    /// </summary>
    public bool GetCapInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.CapsLock);
        }
        return false;
    }

    /// <summary>
    /// 是否按住Control加速
    /// </summary>
    public bool GetCprintInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
        return false;
    }

    /// <summary>
    /// 是否按下Control加速
    /// </summary>
    public bool GetCprintInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.LeftControl);
        }
        return false;
    }

    /// <summary>
    /// 是否按住Space跳躍
    /// </summary>
    public bool GetJumpInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.Space);
        }
        return false;
    }

    /// <summary>
    /// 是否按下Space跳躍
    /// </summary>
    public bool GetJumpInputDown()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    /// <summary>
    /// 取得 Mouse X 的 Axis
    /// </summary>
    public float GetMouseXAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }

    /// <summary>
    /// 取得 Mouse Y 的 Axis
    /// </summary>
    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }

    /// <summary>
    /// 取得滾輪的 Axis
    /// </summary>
    public float GetMouseScrollWheelAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }

    /// <summary>
    /// 是否按下滑鼠左鍵
    /// </summary>
    public bool GetClick()
    {
        return Input.GetMouseButtonDown(0);
    }

    /// <summary>
    /// 取得是否按下滑鼠左鍵(開火)
    /// </summary>
    public bool GetFireInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(0);
        }
        return false;
    }

    /// <summary>
    /// 取得是否持續按下滑鼠左鍵(開火)
    /// </summary>
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }
        return false;
    }

    /// <summary>
    /// 取得是否放開滑鼠左鍵(開火)
    /// </summary>
    public bool GetFireInputUp()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    /// <summary>
    /// 取得是否按下滑鼠右鍵(瞄準)
    /// </summary>
    public bool GetAimInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }

    /// <summary>
    /// 取得滑鼠是否按下Reload
    /// </summary>
    public bool GetReloadInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        return false;
    }

    /// <summary>
    /// 取得是否按下切換武器，向左切換 : -1，向右切換 : 1
    /// </summary>
    public int GetSwichWeaponInput()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                return -1;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                return 1;
            }
        }
        return 0;
    }

    /// <summary>
    /// 確認畫面是否為鎖定狀態
    /// </summary>
    private void CheckCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0 && SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    /// <summary>
    /// 更新鼠標狀態為鎖定
    /// </summary>
    public void CursorStateLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// 顯示鼠標&更新鼠標狀態為未鎖定
    /// </summary>
    public void CursorStateUnlocked()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// 回傳鼠標狀態是否處於鎖定
    /// </summary>
    public bool CanProcessInput()
    {
        // 如果Cursor狀態不在鎖定中就不能處理Input
        return Cursor.lockState == CursorLockMode.Locked;
    }

    #endregion
}
