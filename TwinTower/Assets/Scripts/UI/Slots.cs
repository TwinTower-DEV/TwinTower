using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// UI내에 focus될때 이미지가 변경되는 대부분의 버튼 및 panel에 소속될 슬롯들은 전부 이 클래스를 상속받거나 사용한다.
/// 포커스 될때, 마우스가 클릭할때 변경되는 이미지 및 오브젝트를 배치하여 SetActivate해준다.
/// MenuUI에만 해당된다.
/// 위계질서
/// Slots - DeltableSlot - (LoadSlot, SaveSlot)
/// Slots - (SaveCheck, DeleteCheck)
/// </summary>
public class Slots : MonoBehaviour, ISelectHandler, IDeselectHandler, 
    IPointerEnterHandler, IPointerClickHandler{
    [SerializeField] protected GameObject SelectObject;                 // 포커스 될때 하이라이트시킬 이미지
    [SerializeField] protected GameObject UnSelectObject;               // 포커스를 잃을때 변경될 이미지

    protected MenuUI menuui;
    [SerializeField] protected string nextPanelName;
    public virtual void Awake() {
        Transform trans = transform;
        while (menuui == null) {                                        // menu UI 찾기
            trans = trans.parent;
            menuui = trans.GetComponent<MenuUI>();
        }
    }

    private void OnDisable() {                                          // 비활성화 되면 포커스도 초기화
        SelectObject.SetActive(false);
        UnSelectObject.SetActive(true);
    }

    public void OnSelect(BaseEventData eventData) {
        Select();
    }
    
    public void OnDeselect(BaseEventData eventData) {
        SelectObject.SetActive(false);
        UnSelectObject.SetActive(true);
    }
    
    public void OnPointerEnter(PointerEventData eventData) {            // 마우스 포인터 진입 시 포커스
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerClick(PointerEventData eventData) {
        Click();
    }

    public void Select() {
        SelectObject.SetActive(true);
        UnSelectObject.SetActive(false);
    }

    // 기본적으로 메뉴UI와 통신한다. 
    // menuUI에게 변경할 panel의 이름을 알려주면 해당 panel로 화면이 변경된다.
    public virtual void Click() {
        if(nextPanelName == "") menuui.PrevPanelChange();
        else menuui.SwitchPanelReqeust(this, nextPanelName);
    }
}
