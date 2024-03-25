using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 불러오기 기능 - 바로 불러오기 및 게임 UnPause
/// </summary>
public class LoadSlot : DeletableSlot
{
    public override void Click() {
        string sceneName = PlayerPrefs.GetString(index);
        if (sceneName != "") {
            SceneManager.LoadScene(PlayerPrefs.GetString(index));
            InputManager.Instance.UnPause();
        }
    }
}
