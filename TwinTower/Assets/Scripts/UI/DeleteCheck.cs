using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 삭제하시겠습니까? 창에서 예 버튼에 해당.
/// 누를 경우 MenuUI에 Delete요청
/// 삭제 시킨 후 이전 화면으로
/// </summary>
public class DeleteCheck : Slots
{
    public override void Click() {
        menuui.Delete();
        menuui.PrevPanelChange();
    }
}
