using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// UI_Base를 상속받은 Load 불러오기 창이다.
/// 불러오기 슬롯 3개가 저장되어있다.
/// </summary>
public class UI_Load : UI_Base {
    //private MenuUIManager menuUIManager;
    private int currCursor;
    private static int SLOT_COUNT = 3;

    public override void Init() {
        //menuUIManager = transform.parent.GetComponent<MenuUIManager>();
        
        Bind<Image>(typeof(Load));                                  // 슬롯 바인드
        Bind<Button>(typeof(DeleteButton));                         // 삭제 버튼 바인드
        Bind<TextMeshProUGUI>(typeof(LoadText));                    // 슬롯 text정보 바인드(단계, 날짜)

        UIManager.Instance.InputHandler += KeyInPut;
        UpdateUI();
        // 슬롯 마우스 엔터, 클릭 이벤트 BindEvent
        Get<Image>((int)Load.UnSelectLoad_1).gameObject.BindEvent(()=>LoadEvent((int)Load.UnSelectLoad_1), Define.UIEvent.Click);
        Get<Image>((int)Load.UnSelectLoad_2).gameObject.BindEvent(()=>LoadEvent((int)Load.UnSelectLoad_2), Define.UIEvent.Click);
        Get<Image>((int)Load.UnSelectLoad_3).gameObject.BindEvent(()=>LoadEvent((int)Load.UnSelectLoad_3), Define.UIEvent.Click);
        
        Get<Image>((int)Load.SelectLoad_1).gameObject.BindEvent(()=>LoadEvent((int)Load.UnSelectLoad_1), Define.UIEvent.Click);
        Get<Image>((int)Load.SelectLoad_2).gameObject.BindEvent(()=>LoadEvent((int)Load.UnSelectLoad_2), Define.UIEvent.Click);
        Get<Image>((int)Load.SelectLoad_3).gameObject.BindEvent(()=>LoadEvent((int)Load.UnSelectLoad_3), Define.UIEvent.Click);


        Get<Image>((int)Load.UnSelectLoad_1).gameObject.BindEvent(()=>EnterCursorEvent((int)Load.UnSelectLoad_1), Define.UIEvent.Enter);
        Get<Image>((int)Load.UnSelectLoad_2).gameObject.BindEvent(()=>EnterCursorEvent((int)Load.UnSelectLoad_2), Define.UIEvent.Enter);
        Get<Image>((int)Load.UnSelectLoad_3).gameObject.BindEvent(()=>EnterCursorEvent((int)Load.UnSelectLoad_3), Define.UIEvent.Enter);
        
        // 삭제버튼 클릭시 이벤트 지정
        Get<Button>((int)DeleteButton.DeleteButton_1).gameObject.BindEvent(()=>DeleteEvent((int)DeleteButton.DeleteButton_1), Define.UIEvent.Click);
        Get<Button>((int)DeleteButton.DeleteButton_2).gameObject.BindEvent(()=>DeleteEvent((int)DeleteButton.DeleteButton_2), Define.UIEvent.Click);
        Get<Button>((int)DeleteButton.DeleteButton_3).gameObject.BindEvent(()=>DeleteEvent((int)DeleteButton.DeleteButton_3), Define.UIEvent.Click);
        
        Get<Image>((int)Load.SelectLoad_1).gameObject.SetActive(false);
        Get<Image>((int)Load.SelectLoad_2).gameObject.SetActive(false);
        Get<Image>((int)Load.SelectLoad_3).gameObject.SetActive(false);
        
        // 포커스 설정
        currCursor = 0;
        EnterCursorEvent(currCursor);
    }

    // 저장 슬롯(선택, 비선택 슬롯들)
    enum Load{
        UnSelectLoad_1,
        UnSelectLoad_2,
        UnSelectLoad_3,
        SelectLoad_1,
        SelectLoad_2,
        SelectLoad_3
    }

    enum DeleteButton {
        DeleteButton_1,
        DeleteButton_2,
        DeleteButton_3
    }
    
    enum LoadText {
        USLoadInfoText_1,
        USLoadInfoText_2,
        USLoadInfoText_3,
        SLoadInfoText_1,
        SLoadInfoText_2,
        SLoadInfoText_3
    }

    private void KeyInPut()
    {
        if (!Input.anyKey)
            return;
        if (_uiNum != UIManager.Instance.UINum)
            return;
        if (Input.GetKeyDown(KeyCode.Return)) {                         // 엔터 - 현재 선택된 슬롯 클릭 이벤트 발동
            GameObject go = Get<Image>(currCursor).gameObject;
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
            evt.OnClickHandler.Invoke();
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {                      // 선택 슬롯 변경
            EnterCursorEvent((currCursor + 1) % SLOT_COUNT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            EnterCursorEvent((currCursor - 1 + SLOT_COUNT) % SLOT_COUNT);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {                         // ESC - 뒤로가기
            //menuUIManager.PrevPanelChange();
            UI_SoundEffect();
            UIManager.Instance.InputHandler -= KeyInPut;
            UIManager.Instance.CloseNormalUI(this);
        }
    }
    
    // 슬롯 정보 업데이트
    private void UpdateUI() {
        for (int i = 0; i < SaveLoadController.SLOTCOUNT; i++) {
            Get<TextMeshProUGUI>(i).text = SaveLoadController.GetSaveInfo(i);
            Get<TextMeshProUGUI>(i + SLOT_COUNT).text = SaveLoadController.GetSaveInfo(i);
            if (SaveLoadController.GetSaveInfo(i) == "NO SAVE DATA") {
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

    private void LoadEvent(int idx)
    {
        if(Time.timeScale == 0)
            Time.timeScale = 1;
        UI_SoundEffect();
        DataManager.Instance.saveload.ChangeCurrSaveSlot(idx);
        DataManager.Instance.saveload.Load();
    }

    private void DeleteEvent(int idx) {
        DataManager.Instance.saveload.ChangeCurrSaveSlot(idx);
        //menuUIManager.SwitchPanelPrevSave("SaveDeleteCheckPanel");
        UI_SoundEffect();
        UI_SaveDeleteCheck saveDeleteCheck = UIManager.Instance.ShowNormalUI<UI_SaveDeleteCheck>();
        saveDeleteCheck.PrevPanelUpdateAction += UpdateUI;
    }

    // 마우스 진입시 실행되는 이벤트(포커스 변경)
    void EnterCursorEvent(int currIdx)
    {
        if (SaveLoadController.GetSaveInfo(currIdx) == "NO SAVE DATA")
            return;
        
        UI_SoundEffect();
        Get<Image>(currCursor + SLOT_COUNT).gameObject.SetActive(false);  // 기존것 하이라이트 종료
        Get<Image>(currCursor).gameObject.SetActive(true);
        
        currCursor = currIdx;
        
        Get<Image>(currCursor).gameObject.SetActive(false);         // 선택된것 하이라이트
        Get<Image>(currCursor + SLOT_COUNT).gameObject.SetActive(true);
    }
}
