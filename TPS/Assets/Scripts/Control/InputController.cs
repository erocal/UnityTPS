using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float vertical;
    public float horizontal;

    private void Awake()
    {
        //設定游標狀態 (鎖定)
        Cursor.lockState = CursorLockMode.Locked;
        // 是否顯示游標
        Cursor.visible = false;
    }

    private void Update()
    {
        CheckCursorState();
        //vertical = Input.GetAxis("Vertical");
        //horizontal = Input.GetAxis("Horizontal");
    }

    // 取得WASD的Axis
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

    // 是否按下Sprint加速
    /*public bool GetSprintInput()
    {
        if(CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }*/

    // 是否按住Sprint加速
    /*public bool GetSprintInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.LeftShift);
        }
        return false;
    }*/

    // 是否按下CapsLock加速
    public bool GetCapInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.CapsLock);
        }
        return false;
    }

    // 是否按下Control加速
    public bool GetCprintInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
        return false;
    }

    // 是否按住Control加速
    public bool GetCprintInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.LeftControl);
        }
        return false;
    }

    // 是否按下Space跳躍
    public bool GetJumpInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKey(KeyCode.Space);
        }
        return false;
    }

    // 是否按住Space跳躍
    public bool GetJumpInputDown()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }

    // 取得 Mouse X 的 Axis
    public float GetMouseXAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }

    // 取得 Mouse Y 的 Axis
    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }

    // 取得滾輪的 Axis
    public float GetMouseScrollWheelAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }

    // 取得是否按下滑鼠左鍵
    public bool GetFireInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(0);
        }
        return false;
    }

    // 取得是否持續按下滑鼠左鍵
    public bool GetFireInputHeld()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }
        return false;
    }

    // 取得是否放開滑鼠左鍵
    public bool GetFireInputUp()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    // 取得是否按下滑鼠右鍵
    public bool GetAimInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }

    // 取得滑鼠是否按下Reload
    public bool GetReloadInputDown()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        return false;
    }

    // 取得是否按下切換武器
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

    private void CheckCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void CursorStateLocked()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool CanProcessInput()
    {
        // 如果Cursor狀態不在鎖定中就不能處理Input
        return Cursor.lockState == CursorLockMode.Locked;
    }
}
