using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI_Base를 상속받은 저장 삭제 확인 창이다.
/// 예/아니오 버튼 2개가 있다.
/// 저장할 슬롯 정보를 확인하기 위한 Text 1개 있다.
/// 맨앞에 있을때 부르는 함수 있어야 할듯
/// </summary>
public class UI_SaveDeleteCheck : UI_Base
{
    //private MenuUIManager menuUIManager;
    private int currCursor;
    private static int BUTTON_COUNT = 2;
    public Action PrevPanelUpdateAction = null;

    public override void Init() {
        //menuUIManager = transform.parent.GetComponent<MenuUIManager>();
        
        Bind<Image>(typeof(Check));
        Bind<TextMeshProUGUI>(typeof(Save));
        
        UIManager.Instance.InputHandler += KeyInPut;
        
        Get<Image>((int)Check.SelectYes).gameObject.BindEvent(YesEvent, Define.UIEvent.Click);
        Get<Image>((int)Check.SelectNo).gameObject.BindEvent(NoEvent, Define.UIEvent.Click);
        
        Get<Image>((int)Check.UnSelectYes).gameObject.BindEvent(YesEvent, Define.UIEvent.Click);
        Get<Image>((int)Check.UnSelectNo).gameObject.BindEvent(NoEvent, Define.UIEvent.Click);
        
        Get<Image>((int)Check.UnSelectYes).gameObject.BindEvent(()=>EnterCursorEvent((int)Check.UnSelectYes), Define.UIEvent.Enter);
        Get<Image>((int)Check.UnSelectNo).gameObject.BindEvent(()=>EnterCursorEvent((int)Check.UnSelectNo), Define.UIEvent.Enter);

        Get<Image>((int)Check.SelectYes).gameObject.SetActive(false);
        Get<Image>((int)Check.SelectNo).gameObject.SetActive(false);

        Get<TextMeshProUGUI>((int)Save.SaveInfo).text =
            SaveLoadController.GetSaveInfo(DataManager.Instance.saveload.GetCurrSaveSlot());
        
        Get<Image>((int)Check.SelectYes).gameObject.SetActive(false);
        Get<Image>((int)Check.SelectNo).gameObject.SetActive(false);

        currCursor = 0;
        EnterCursorEvent(currCursor);
    }

    enum Check{
        UnSelectYes,
        UnSelectNo,
        SelectYes,
        SelectNo,
    }

    enum Save {
        SaveInfo
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

        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            EnterCursorEvent((currCursor + 1) % BUTTON_COUNT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            EnterCursorEvent((currCursor - 1 + BUTTON_COUNT) % BUTTON_COUNT);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UIManager.Instance.InputHandler -= KeyInPut;
            UIManager.Instance.CloseNormalUI(this);
        }
    }

    private void YesEvent() {
        UI_SoundEffect();
        UIManager.Instance.InputHandler -= KeyInPut;
        DataManager.Instance.saveload.Delete();
        PrevPanelUpdateAction.Invoke();
        UIManager.Instance.CloseNormalUI(this);
    }

    private void NoEvent() {
        UI_SoundEffect();
        UIManager.Instance.InputHandler -= KeyInPut;
        UIManager.Instance.CloseNormalUI(this);    
    }

    void EnterCursorEvent(int currIdx) {
        UI_SoundEffect();
        Get<Image>(currCursor + BUTTON_COUNT).gameObject.SetActive(false);  // 기존것 하이라이트 종료
        Get<Image>(currCursor).gameObject.SetActive(true);
        
        currCursor = currIdx;
        
        Get<Image>(currCursor).gameObject.SetActive(false);         // 선택된것 하이라이트
        Get<Image>(currCursor + BUTTON_COUNT).gameObject.SetActive(true);
    }
}
