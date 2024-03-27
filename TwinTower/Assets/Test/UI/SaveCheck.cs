using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// SaveSlot의 하나의 슬롯이 가지게 될 컴포넌트
/// 포커스 되는 슬롯을 기준으로 포커스가 된다면 저장해 둔 SelectObject활성, UnSelectObject비활성 시킨다.
/// 또, 마우스 포인터로 가져다 대면 포커스가 그 슬롯으로 변경된다.
/// 포커스 된 상황에서 엔터 또는 클릭을 할 경우 해당 슬롯에 저장되어야 한다.
/// </summary>
public class SaveCheck : Slots{
    public override void Click() {
        menuui.Save();
        menuui.PrevPanelChange();
    }
}
