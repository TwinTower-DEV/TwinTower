using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DeletableSlot을 상속 - 삭제 기능 존재
/// 저장 기능
/// </summary>
public class SaveSlot : DeletableSlot {
    public void Save() {
        TextMeshProUGUI SelectText =SelectObject.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI UnSelectText =UnSelectObject.GetComponentInChildren<TextMeshProUGUI>();

        string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string saveStage = SceneManager.GetActiveScene().name;
        
        PlayerPrefs.SetString(index, saveStage);
        PlayerPrefs.SetString(index + "Date", date);

        SelectText.text = "Save #" + index + " - " + saveStage + ", " + date;
        UnSelectText.text = "Save #" + index + " - " + saveStage + ", " + date;

        updateUI();
    }
}
