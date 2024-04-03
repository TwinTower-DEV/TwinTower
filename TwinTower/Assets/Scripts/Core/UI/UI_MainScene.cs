using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace TwinTower
{
    public class UI_MainScene : UI_Base
    {
        const int BUTTON_COUNT = (int)Images.Exit + 1;
        private Action[] _actions = new Action[BUTTON_COUNT];
        private int currcoursor;
        
        public override void Init()
        {
            Bind<Image>(typeof(Images));

            UIManager.Instance.InputHandler -= KeyInPut;
            UIManager.Instance.InputHandler += KeyInPut;
            
            Get<Image>((int)Images.newGame).gameObject.BindEvent(NewGame, Define.UIEvent.Click);
            Get<Image>((int)Images.Setting).gameObject.BindEvent(Setting, Define.UIEvent.Click);
            Get<Image>((int)Images.Continue).gameObject.BindEvent(Continue, Define.UIEvent.Click);
            Get<Image>((int)Images.Exit).gameObject.BindEvent(Exit, Define.UIEvent.Click);
            
            Get<Image>((int)Images.newGame).gameObject.BindEvent(()=>ChangeCursor((int)Images.newGame), Define.UIEvent.Enter);
            Get<Image>((int)Images.Setting).gameObject.BindEvent(()=>ChangeCursor((int)Images.Setting), Define.UIEvent.Enter);
            Get<Image>((int)Images.Continue).gameObject.BindEvent(()=>ChangeCursor((int)Images.Continue), Define.UIEvent.Enter);
            Get<Image>((int)Images.Exit).gameObject.BindEvent(()=>ChangeCursor((int)Images.Exit), Define.UIEvent.Enter);
            
            Get<Image>((int)Images.newGame).gameObject.BindEvent(()=>ExitCursor((int)Images.newGame), Define.UIEvent.Exit);
            Get<Image>((int)Images.Setting).gameObject.BindEvent(()=>ExitCursor((int)Images.Setting), Define.UIEvent.Exit);
            Get<Image>((int)Images.Continue).gameObject.BindEvent(()=>ExitCursor((int)Images.Continue), Define.UIEvent.Exit);
            Get<Image>((int)Images.Exit).gameObject.BindEvent(()=>ExitCursor((int)Images.Exit), Define.UIEvent.Exit);
            
            currcoursor = 0;
            ChangeCursor(currcoursor);
            _actions[0] = NewGame;
            _actions[1] = Continue;
            _actions[2] = Setting;
            _actions[3] = Exit;
        }

        enum Images
        {   
            newGame,
            Continue,
            Setting,
            Exit
        }

        private void KeyInPut()
        {
            if (!Input.anyKey)
                return;
            
            if (_uiNum != UIManager.Instance.UINum)
                return;
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                _actions[currcoursor].Invoke();
                return;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                ChangeCursor((currcoursor + 1) % BUTTON_COUNT);
                return;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                ChangeCursor((currcoursor - 1 + BUTTON_COUNT) % BUTTON_COUNT);
            }
        }
        void NewGame()
        {
            UIManager.Instance.InputHandler -= KeyInPut;
            StartCoroutine(ScreenManager.Instance.NextSceneload(DataManager.Instance.StageInfovalue.nextStage));
            
            Debug.Log("New Game Start");
        }
        void Setting()
        {
            UIManager.Instance.ShowNormalUI<UI_SettingScene>();
            Debug.Log("Setting Open");
        }

        void Continue()
        {
            Debug.Log("Continue");
        }

        void Exit()
        {
            Debug.Log("Game End");
        }

        void EnterCoursor(int nextidx)
        {
            string curcolor = "#FFFFFF";
            Color curnewcolor;
            if (ColorUtility.TryParseHtmlString(curcolor, out curnewcolor))
            {
                Get<Image>(currcoursor).gameObject.GetComponentInChildren<TextMeshProUGUI>().color = curnewcolor;
            }
            currcoursor = nextidx;
            string color = "#D86ECC";
            Color newcolor;
            if (ColorUtility.TryParseHtmlString(color, out newcolor))
            {
                Debug.Log(currcoursor + " " + Get<Image>(currcoursor).gameObject.name);
                Get<Image>(currcoursor).gameObject.GetComponentInChildren<TextMeshProUGUI>().color = newcolor;
            }
        }
        
        void ChangeCursor(int nextidx)
        {
            ExitCursor(currcoursor);
            EnterCoursor(nextidx);
            Debug.Log("ChangeCursor");
        }

        void ExitCursor(int idx)
        {
            
            Debug.Log("ExitCursor");
        }
    }
}