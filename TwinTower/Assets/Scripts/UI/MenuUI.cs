using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// MenuUI의 전체적인 컨트롤러
/// 화면의 전환과 UI간의 통신을 담당한다.
/// </summary>
public class MenuUI : MonoBehaviour {
    private Stack<Slots> SlotStack = new Stack<Slots>();                // 이전 화면으로 돌아갈때 사용되는 Stack
    private Dictionary<string, GameObject> PanelDict = new Dictionary<string, GameObject>();    // Panel변경에 사용될 dict
    private string currentPanel;
    
    // 우선 MenuUI하위에 있는 모든 Panel을 담아주고, 그 Panel의 이름을 Key로 Dict에 담아준다.
    // 추후 Panel의 교체에 SetAcitvate로 이루어질 예정이다.
    private void Awake(){               
        for (int i = 0; i < transform.childCount; i++) {
            GameObject childObject = transform.GetChild(i).gameObject;
            PanelDict[childObject.name] = childObject;
        }
    }

    // 키 입력 이벤트를 담당
    // 엔터와 ESC키 입력 이벤트 처리
    // 엔터 - 포커스된 슬롯 클릭 처리
    // ESC - 뒤로 가기 및 인게임 재생
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            GameObject currentSelectObject = EventSystem.current.currentSelectedGameObject;
            currentSelectObject.GetComponent<Slots>().Click();
            
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            PrevPanelChange();
        }
    }
    
    // 뒤로가기 - Stack을 이용
    public void PrevPanelChange() {
        if(SlotStack.Count == 0) InputManager.Instance.UnPause();
        else {
            Slots prevSlot = SlotStack.Pop();
            Transform trans = prevSlot.transform;
            MenuPanel menupanel = trans.GetComponent<MenuPanel>();

            while (menupanel == null) {
                trans = trans.parent;
                menupanel = trans.GetComponent<MenuPanel>();
            }

            SwitchPanel(menupanel.gameObject.name);
        }
    }

    // Stack에 담을 슬롯 저장 및 화면 교체 명령
    public void SwitchPanelReqeust(Slots SelectSlot, string PanelName) {
        SlotStack.Push(SelectSlot);
        SwitchPanel(PanelName);
    }

    // 화면 교체
    public void SwitchPanel(string PanelName) {
        foreach (var key in PanelDict.Keys) {
            PanelDict[key].SetActive(false);
        }
        PanelDict[PanelName].SetActive(true);
    }

    // 선택된 슬롯에 저장을 시켜줌.(예/아니오 창에서 정보를 전달해주는 역할)
    public void Save() {
        SaveSlot saveslot = (SaveSlot)SlotStack.Peek();
        saveslot.Save();
    }

    // 선택된 슬롯 삭제.(예/아니오 창에서 정보를 전달해주는 역할)
    public void Delete() {
        DeletableSlot saveslot = (DeletableSlot)SlotStack.Peek();
        saveslot.Delete();
    }

    // 선택된 슬롯 정보 전달.(예/아니오 창에 정보를 전달해주는 역할)
    public string getslotIndex() {
        DeletableSlot saveslot = (DeletableSlot)SlotStack.Peek();
        return saveslot.updateUI();
    }
}
