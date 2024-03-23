using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 삭제 가능한 슬롯들(주로 저장과 로드 슬롯)
/// 저장과 삭제가 될 때 슬롯 정보 업데이트 내용과 삭제기능을 담고 있다.
/// </summary>
public class DeletableSlot : Slots
{
    [SerializeField] public string index;
    TextMeshProUGUI SelectText;                     // 날짜 저장
    TextMeshProUGUI UnSelectText;


    public override void Awake() {
        base.Awake();
        SelectText = SelectObject.GetComponentInChildren<TextMeshProUGUI>();
        UnSelectText =UnSelectObject.GetComponentInChildren<TextMeshProUGUI>();

        updateUI();
    }

    private void OnEnable() {
        updateUI();
    }

    // 슬롯에 정보 업데이트 주로 단계와 날짜가 담겨져 있다.
    public string updateUI() {
        string date = PlayerPrefs.GetString(index + "Date");
        string saveStage = PlayerPrefs.GetString(index);

        if (date == "") {
            SelectText.text = "NO SAVE DATA";
            UnSelectText.text = "NO SAVE DATA";
        }
        else {
            SelectText.text = "Save #" + index + " - " + saveStage + ", " + date;
            UnSelectText.text = "Save #" + index + " - " + saveStage + ", " + date;
        }

        return SelectText.text;
    }

    // 삭제
    public void Delete() {
        PlayerPrefs.SetString(index, null);
        PlayerPrefs.SetString(index + "Date", null);

        updateUI();
    }

    // 삭제 키를 눌렀을때 전환되는 화면 전달.
    public void DeleteSave() {
        if(PlayerPrefs.GetString(index + "Date") != "") menuui.SwitchPanelReqeust(this, "SaveDeleteCheckPanel");
    }
}
