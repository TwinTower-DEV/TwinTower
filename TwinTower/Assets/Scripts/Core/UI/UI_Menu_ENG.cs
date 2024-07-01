using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TwinTower;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI_Base를 상속받은 Menu창이다.
/// 저장하기, 불러오기, 세팅, 메인메뉴로 가는 버튼 4개가 있다.
/// </summary>
public class UI_Menu_ENG : UI_Base {
    //private MenuUIManager menuUIManager;
    private int currCursor;
    private static int BUTTON_COUNT = 4;

    private void Update() {
       // KeyInPut();
    }

    public override void Init() {
        //menuUIManager = transform.parent.GetComponent<MenuUIManager>();
        
        Bind<Image>(typeof(Menu));                  // 각 버튼 Bind
        UIManager.Instance.InputHandler -= KeyInPut;
        UIManager.Instance.InputHandler += KeyInPut;
        // 클릭 이벤트
        Get<Image>((int)Menu.UnSelectSave).gameObject.BindEvent(SaveEvent, Define.UIEvent.Click);
        Get<Image>((int)Menu.UnSelectLoad).gameObject.BindEvent(LoadEvent, Define.UIEvent.Click);
        Get<Image>((int)Menu.UnSelectSetting).gameObject.BindEvent(SettingEvent, Define.UIEvent.Click);
        Get<Image>((int)Menu.UnSelectMainMenu).gameObject.BindEvent(MainMenuEvent, Define.UIEvent.Click);
        
        Get<Image>((int)Menu.SelectSave).gameObject.BindEvent(SaveEvent, Define.UIEvent.Click);
        Get<Image>((int)Menu.SelectLoad).gameObject.BindEvent(LoadEvent, Define.UIEvent.Click);
        Get<Image>((int)Menu.SelectSetting).gameObject.BindEvent(SettingEvent, Define.UIEvent.Click);
        Get<Image>((int)Menu.SelectMainMenu).gameObject.BindEvent(MainMenuEvent, Define.UIEvent.Click);

        // Mouse Enter 이벤트
        Get<Image>((int)Menu.UnSelectSave).gameObject.BindEvent(()=>EnterCursorEvent((int)Menu.UnSelectSave), Define.UIEvent.Enter);
        Get<Image>((int)Menu.UnSelectLoad).gameObject.BindEvent(()=>EnterCursorEvent((int)Menu.UnSelectLoad), Define.UIEvent.Enter);
        Get<Image>((int)Menu.UnSelectSetting).gameObject.BindEvent(()=>EnterCursorEvent((int)Menu.UnSelectSetting), Define.UIEvent.Enter);
        Get<Image>((int)Menu.UnSelectMainMenu).gameObject.BindEvent(()=>EnterCursorEvent((int)Menu.UnSelectMainMenu), Define.UIEvent.Enter);
        
        // 초기 설정
        Get<Image>((int)Menu.SelectSave).gameObject.SetActive(false);
        Get<Image>((int)Menu.SelectLoad).gameObject.SetActive(false);
        Get<Image>((int)Menu.SelectSetting).gameObject.SetActive(false);
        Get<Image>((int)Menu.SelectMainMenu).gameObject.SetActive(false);

        currCursor = 0;
        EnterCursorEvent(currCursor);
    }

    enum Menu{
        UnSelectSave,
        UnSelectLoad,
        UnSelectSetting,
        UnSelectMainMenu,
        SelectSave,
        SelectLoad,
        SelectSetting,
        SelectMainMenu,
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
            EnterCursorEvent((currCursor + 1) % BUTTON_COUNT);
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            EnterCursorEvent((currCursor - 1 + BUTTON_COUNT) % BUTTON_COUNT);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance.InputHandler -= KeyInPut;
            Time.timeScale = 1;
            UI_SoundEffect();
            InputController.Instance.GainControl();
            UIManager.Instance.CloseNormalUI(this);
            //menuUIManager.PrevPanelChange();
        }
    }

    private void SaveEvent() {
        //menuUIManager.SwitchPanelPrevSave("SavePanel");
        UI_ClickSoundEffect();
        UIManager.Instance.ShowNormalUI<UI_Save_ENG>();
    }
    
    private void LoadEvent() {
        //menuUIManager.SwitchPanelPrevSave("LoadPanel");
        UI_ClickSoundEffect();
        UIManager.Instance.ShowNormalUI<UI_Load_ENG>();
    }
    
    private void SettingEvent()
    {
        UIManager.Instance.ShowNormalUI<UI_SettingScene_ENG>();
        UI_ClickSoundEffect();
        Debug.Log("Enter Setting");
    }

    private void MainMenuEvent() {
        UI_ClickSoundEffect();
        InputManager.Destroys();
        Time.timeScale = 1;
        StartCoroutine(ScreenManager.Instance.NextSceneload("MainScene"));
        //UIManager.Instance.CloseNormalUI(this);
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
