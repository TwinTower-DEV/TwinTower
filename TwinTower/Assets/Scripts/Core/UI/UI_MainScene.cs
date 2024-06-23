using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace TwinTower
{
    public class UI_MainScene : UI_Base
    {
        const int BUTTON_COUNT = 4;
        private Action[] _actions = new Action[BUTTON_COUNT];
        private int currcoursor;
        [SerializeField] private AudioClip MainSceneBGM;
        [SerializeField] private AudioClip IngameBGM;
        public override void Init()
        {
            SoundManager.Instance.Play(MainSceneBGM, Define.Sound.Bgm);
            Bind<Image>(typeof(Images));

            UIManager.Instance.InputHandler -= KeyInPut;
            UIManager.Instance.InputHandler += KeyInPut;
            
            Get<Image>((int)Images.SelectNewGame).gameObject.BindEvent(NewGame, Define.UIEvent.Click);
            Get<Image>((int)Images.SelectNewGame).gameObject.SetActive(false);
            
            Get<Image>((int)Images.SelectSetting).gameObject.BindEvent(Setting, Define.UIEvent.Click);
            Get<Image>((int)Images.SelectSetting).gameObject.SetActive(false);
            
            Get<Image>((int)Images.SelectContinue).gameObject.BindEvent(Continue, Define.UIEvent.Click);
            Get<Image>((int)Images.SelectContinue).gameObject.SetActive(false);
            
            Get<Image>((int)Images.SelectExit).gameObject.BindEvent(Exit, Define.UIEvent.Click);
            Get<Image>((int)Images.SelectExit).gameObject.SetActive(false);
            

            Get<Image>((int)Images.NoSelectNewGame).gameObject.BindEvent(()=>ChangeCursor((int)Images.SelectNewGame), Define.UIEvent.Enter);
            Get<Image>((int)Images.NoSelectSetting).gameObject.BindEvent(()=>ChangeCursor((int)Images.SelectSetting), Define.UIEvent.Enter);
            Get<Image>((int)Images.NoSelectContinue).gameObject.BindEvent(()=>ChangeCursor((int)Images.SelectContinue), Define.UIEvent.Enter);
            Get<Image>((int)Images.NoSelectExit).gameObject.BindEvent(()=>ChangeCursor((int)Images.SelectExit), Define.UIEvent.Enter);
            
            currcoursor = 0;
            ChangeCursor(currcoursor);
            _actions[0] = NewGame;
            _actions[1] = Continue;
            _actions[2] = Setting;
            _actions[3] = Exit;
        }

        enum Images
        {   
            SelectNewGame,
            SelectContinue,
            SelectSetting,
            SelectExit,
            NoSelectNewGame,
            NoSelectContinue,
            NoSelectSetting,
            NoSelectExit
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
            StartCoroutine(ScreenManager.Instance.NextSceneload());
            
        }
        void Setting()
        {
            UIManager.Instance.ShowNormalUI<UI_SettingScene>();
        }

        void Continue()
        {
            UIManager.Instance.ShowNormalUI<UI_Load>();
        }

        void Exit()
        {
            UIManager.Instance.ShowNormalUI<UI_ExitCheck>();
        }

        void EnterCoursor(int nextidx)
        {
            Get<Image>(currcoursor).gameObject.SetActive(false);
            Get<Image>(currcoursor + BUTTON_COUNT).gameObject.SetActive(true);

            currcoursor = nextidx;
            
            Get<Image>(currcoursor).gameObject.SetActive(true);
            Get<Image>(currcoursor + BUTTON_COUNT).gameObject.SetActive(false);
        }
        
        void ChangeCursor(int nextidx)
        {
            ExitCursor(currcoursor);
            EnterCoursor(nextidx);
        }

        void ExitCursor(int idx)
        {
            
        }

        public int Test()
        {
            EnterCoursor(1);
            EnterCoursor(2);
            EnterCoursor(3);

            return currcoursor;
        }
    }
}