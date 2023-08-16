using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [Header("開始畫面的UI")]
    [SerializeField] GameObject startGameUI;

    InputController input;

    private void Awake()
    {
        input = GameManagerSingleton.Instance.InputController;
        startGameUI?.SetActive(true);

        input.CursorStateUnlocked();
        Time.timeScale = 0;
    }

    public void GameStart()
    {
        input.CursorStateLocked();
        Time.timeScale = 1;
        startGameUI.SetActive(false);
    }

    public bool CheckGameStart()
    {
        return startGameUI.activeSelf;
    }
}
