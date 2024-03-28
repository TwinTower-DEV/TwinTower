using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;
/// <summary>
/// Manager완성 전 임시 Menu를 위한 메니저이다.
/// Panel전환, SaveLoadController와 통신을 담당한다.
/// </summary>
public class MenuUIManager : MonoBehaviour
{
    private Stack<string> UIStack = new Stack<string>();                // 이전 화면으로 돌아갈때 사용되는 Stack
    private Dictionary<string, GameObject> PanelDict = new Dictionary<string, GameObject>();    // Panel변경에 사용될 dict
    private string currentPanel;
    public SaveLoadController saveloadController = new SaveLoadController();
    
    // 우선 MenuUI하위에 있는 모든 Panel을 담아주고, 그 Panel의 이름을 Key로 Dict에 담아준다.
    // 추후 Panel의 교체에 SetAcitvate로 이루어질 예정이다.
    private void Awake(){               
        for (int i = 0; i < transform.childCount; i++) {
            GameObject childObject = transform.GetChild(i).gameObject;
            PanelDict[childObject.name] = childObject;
        }

        currentPanel = transform.GetChild(0).name;
    }

    private void Update() {
        if(UIStack.Count != 0) Debug.Log(UIStack.Peek());
    }
    
    // 뒤로가기 - Stack을 이용
    public void PrevPanelChange() {
        if(UIStack.Count == 0) InputManager.Instance.UnPause();
        else {
            string prevUI = UIStack.Pop();
            currentPanel = prevUI;
            SwitchPanel(prevUI);
        }
    }

    // Stack에 담을 슬롯 저장 및 화면 교체 명령
    public void SwitchPanelPrevSave(string PanelName) {
        UIStack.Push(currentPanel);
        currentPanel = PanelName;
        SwitchPanel(PanelName);
    }

    // 화면 교체
    public void SwitchPanel(string PanelName) {
        foreach (var key in PanelDict.Keys) {
            PanelDict[key].SetActive(false);
        }
        PanelDict[PanelName].SetActive(true);
    }
}
