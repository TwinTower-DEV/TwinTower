using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TwinTower;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 저장과 삭제, 불러오기를 담당한다.
/// </summary>
public class SaveLoadController {
    public static int SLOTCOUNT = 3;
    private int currSaveSlot;

    // 현재 선택된 슬롯의 인덱스 set
    public void ChangeCurrSaveSlot(int idx) {
        if (idx >= 3) throw new Exception("세이브 슬롯 설정이 이상함");
        currSaveSlot = idx;
    }
    
    public void Load() {
        if (PlayerPrefs.GetString(currSaveSlot.ToString()) == "") return;
        ManagerSet.UI.Load(PlayerPrefs.GetString(currSaveSlot.ToString()));
    }

    public void Save() {
        string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
        string saveStage = SceneManager.GetActiveScene().name;
        
        PlayerPrefs.SetString(currSaveSlot.ToString(), saveStage);
        PlayerPrefs.SetString(currSaveSlot.ToString() + "Date", date);
    }

    public void Save(string stage)
    {
        string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
        
        PlayerPrefs.SetString(currSaveSlot.ToString(), stage);
        PlayerPrefs.SetString(currSaveSlot.ToString() + "Date", date);
    }

    public void Delete() {
        PlayerPrefs.SetString(currSaveSlot.ToString(), null);
        PlayerPrefs.SetString(currSaveSlot.ToString() + "Date", null);
    }

    public int GetCurrSaveSlot() {
        return currSaveSlot;
    }

    // 저장 정보 슬롯에 보여주기 위한 정보들 format
    public static string GetSaveInfo(int idx) {
        string date = PlayerPrefs.GetString(idx.ToString() + "Date");
        string saveStage = PlayerPrefs.GetString(idx.ToString());

        string retString;
        if (date == "") retString = "NO SAVE DATA";
        else {
            retString = "Save #" + (idx + 1).ToString() + " - " + saveStage + ", " + date;
        }

        return retString;
    }
}
