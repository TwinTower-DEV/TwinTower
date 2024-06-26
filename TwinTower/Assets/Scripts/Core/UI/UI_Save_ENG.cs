using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
/// <summary>
/// UI_Base를 상속받은 저장하기 창이다.
/// 저장 슬롯 총 3개의 버튼이 있다.
/// </summary>
public class UI_Save_ENG : UI_Base {
    //private MenuUIManager menuUIManager;
    private int currCursor;
    private static int SLOT_COUNT = 3;

    public override void Init() {
        Bind<Image>(typeof(Save));                             // 슬롯 바인드
        Bind<Button>(typeof(DeleteButton));                    // 삭제 버튼 바인드    
        Bind<TextMeshProUGUI>(typeof(SaveText));               // 슬롯 text정보 바인드(단계, 날짜)        
        UpdateUI();
        UIManager.Instance.InputHandler += KeyInPut;
        
        // 슬롯 마우스 엔터, 클릭 이벤트 BindEvent
        Get<Image>((int)Save.UnSelectSave_1).gameObject.BindEvent(()=>SaveEvent((int)Save.UnSelectSave_1), Define.UIEvent.Click);
        Get<Image>((int)Save.UnSelectSave_2).gameObject.BindEvent(()=>SaveEvent((int)Save.UnSelectSave_2), Define.UIEvent.Click);
        Get<Image>((int)Save.UnSelectSave_3).gameObject.BindEvent(()=>SaveEvent((int)Save.UnSelectSave_3), Define.UIEvent.Click);
        
        Get<Image>((int)Save.SelectSave_1).gameObject.BindEvent(()=>SaveEvent((int)Save.UnSelectSave_1), Define.UIEvent.Click);
        Get<Image>((int)Save.SelectSave_2).gameObject.BindEvent(()=>SaveEvent((int)Save.UnSelectSave_2), Define.UIEvent.Click);
        Get<Image>((int)Save.SelectSave_3).gameObject.BindEvent(()=>SaveEvent((int)Save.UnSelectSave_3), Define.UIEvent.Click);


        Get<Image>((int)Save.UnSelectSave_1).gameObject.BindEvent(()=>EnterCursorEvent((int)Save.UnSelectSave_1), Define.UIEvent.Enter);
        Get<Image>((int)Save.UnSelectSave_2).gameObject.BindEvent(()=>EnterCursorEvent((int)Save.UnSelectSave_2), Define.UIEvent.Enter);
        Get<Image>((int)Save.UnSelectSave_3).gameObject.BindEvent(()=>EnterCursorEvent((int)Save.UnSelectSave_3), Define.UIEvent.Enter);
        
        // 삭제버튼 클릭시 이벤트 지정
        Get<Button>((int)DeleteButton.DeleteButton_1).gameObject.BindEvent(()=>DeleteEvent((int)DeleteButton.DeleteButton_1), Define.UIEvent.Click);
        Get<Button>((int)DeleteButton.DeleteButton_2).gameObject.BindEvent(()=>DeleteEvent((int)DeleteButton.DeleteButton_2), Define.UIEvent.Click);
        Get<Button>((int)DeleteButton.DeleteButton_3).gameObject.BindEvent(()=>DeleteEvent((int)DeleteButton.DeleteButton_3), Define.UIEvent.Click);

        // 초기 설정
        Get<Image>((int)Save.SelectSave_1).gameObject.SetActive(false);
        Get<Image>((int)Save.SelectSave_2).gameObject.SetActive(false);
        Get<Image>((int)Save.SelectSave_3).gameObject.SetActive(false);
        
        currCursor = 1;
        EnterCursorEvent(currCursor);
    }

    enum Save{
        UnSelectSave_1,
        UnSelectSave_2,
        UnSelectSave_3,
        SelectSave_1,
        SelectSave_2,
        SelectSave_3
    }

    enum DeleteButton {
        DeleteButton_1,
        DeleteButton_2,
        DeleteButton_3
    }
    
    enum SaveText {
        USSaveInfoText_1,
        USSaveInfoText_2,
        USSaveInfoText_3,
        SSaveInfoText_1,
        SSaveInfoText_2,
        SSaveInfoText_3
    }

    private void KeyInPut()
    {
        if (!Input.anyKey)
            return;
        if (_uiNum != UIManager.Instance.UINum)
            return;
        if (Input.GetKeyDown(KeyCode.Return)) {
            GameObject go = Get<Image>(currCursor).gameObject;
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
            evt.OnClickHandler.Invoke();
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            EnterCursorEvent((currCursor + 1) % SLOT_COUNT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            EnterCursorEvent((currCursor - 1 + SLOT_COUNT) % SLOT_COUNT);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
           // menuUIManager.PrevPanelChange();
           UI_SoundEffect();
           UIManager.Instance.InputHandler -= KeyInPut;
           UIManager.Instance.CloseNormalUI(this);
        }
    }

    private void UpdateUI() {
        for (int i = 0; i < SaveLoadController.SLOTCOUNT; i++) {
            Get<TextMeshProUGUI>(i).text = SaveLoadController.GetSaveInfo(i);
            Get<TextMeshProUGUI>(i + SLOT_COUNT).text = SaveLoadController.GetSaveInfo(i);

            if (SaveLoadController.GetSaveInfo(i) == "NO SAVE DATA" || i == 0) {
                Get<Button>(i).gameObject.SetActive(false);
                Color newcolor;
                if (ColorUtility.TryParseHtmlString("#7F7F7F", out newcolor))
                {
                    Util.FindChild<Image>(Get<Image>(i).gameObject).color = newcolor;
                }
            }
            else Get<Button>(i).gameObject.SetActive(true);
        }
    }

    private void SaveEvent(int idx) {
        UI_SoundEffect();
        DataManager.Instance.saveload.ChangeCurrSaveSlot(idx);
       // menuUIManager.saveloadController.ChangeCurrSaveSlot(idx);
       UI_SaveCheck_ENG savecheck = UIManager.Instance.ShowNormalUI<UI_SaveCheck_ENG>();
       savecheck.PrevPanelUpdateAction += UpdateUI;
    }

    private void DeleteEvent(int idx) {
        UI_SoundEffect();
        DataManager.Instance.saveload.ChangeCurrSaveSlot(idx);
        //menuUIManager.saveloadController.ChangeCurrSaveSlot(idx);
        UI_SaveDeleteCheck_ENG saveDeleteCheck = UIManager.Instance.ShowNormalUI<UI_SaveDeleteCheck_ENG>();
        saveDeleteCheck.PrevPanelUpdateAction += UpdateUI;
    }

    void EnterCursorEvent(int currIdx) {
        UI_SoundEffect();
        Get<Image>(currCursor + SLOT_COUNT).gameObject.SetActive(false);  // 기존것 하이라이트 종료
        Get<Image>(currCursor).gameObject.SetActive(true);

        if (currIdx == 0) currIdx++;
        
        currCursor = currIdx;
        
        Get<Image>(currCursor).gameObject.SetActive(false);         // 선택된것 하이라이트
        Get<Image>(currCursor + SLOT_COUNT).gameObject.SetActive(true);
    }
}
