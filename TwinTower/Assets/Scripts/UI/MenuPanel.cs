using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 슬롯들을 담는 Panel들 - 포커스 지정 및 정보 전달 역할
/// </summary>
public class MenuPanel : MonoBehaviour
{
    private void OnEnable() {
        TextMeshProUGUI datetext = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        if (datetext != null) {
            string viewSlotIdx = transform.parent.GetComponent<MenuUI>().getslotIndex();
            datetext.text = viewSlotIdx;
        }
        if(transform.GetChild(0))
        GetComponentInChildren<Selectable>().Select();
        GetComponentInChildren<Slots>().Select();
    }
}
