using System;
using UnityEngine;
using UnityEngine.UI;


namespace TwinTower
{
    public class UI_ExitCheck : UI_Base
    {
        private int cursor;
        private const int Button_Count = 2;
        private Action[] _actions = new Action[Button_Count];

        enum Check
        {
            SelectYes,
            SelectNo,
            UnSelectYes,
            UnSelectNo,
        }
        
        public override void Init()
        {
            Bind<Image>(typeof(Check));

            UIManager.Instance.InputHandler -= KeyInput;
            UIManager.Instance.InputHandler += KeyInput;
            
            Get<Image>((int)Check.SelectYes).gameObject.BindEvent(YesClickEvent, Define.UIEvent.Click);
            Get<Image>((int)Check.SelectYes).gameObject.SetActive(false);

            Get<Image>((int)Check.SelectNo).gameObject.BindEvent(NoClickEvent, Define.UIEvent.Click);
            Get<Image>((int)Check.SelectNo).gameObject.SetActive(false);

            Get<Image>((int)Check.UnSelectYes).gameObject.BindEvent(()=>EnterCursorEvent((int)Check.SelectYes), Define.UIEvent.Enter);
            Get<Image>((int)Check.UnSelectNo).gameObject.BindEvent(()=>EnterCursorEvent((int)Check.SelectNo), Define.UIEvent.Enter);

            _actions[0] = YesClickEvent;
            _actions[1] = NoClickEvent;
            cursor = 0;
            EnterCursorEvent(cursor);
        }

        private void KeyInput()
        {
            if (!Input.anyKey)
                return;
            if(_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.InputHandler -= KeyInput;
                UIManager.Instance.CloseNormalUI(this);
            }
        }

        private void YesClickEvent()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif

        }

        private void NoClickEvent()
        {
            UIManager.Instance.InputHandler -= KeyInput;
            UIManager.Instance.CloseNormalUI(this);
        }
        
        void EnterCursorEvent(int currIdx) {
            UI_SoundEffect();
            Get<Image>(cursor + Button_Count).gameObject.SetActive(true);  // 기존것 하이라이트 종료
            Get<Image>(cursor).gameObject.SetActive(false);
        
            cursor = currIdx;
        
            Get<Image>(cursor).gameObject.SetActive(true);         // 선택된것 하이라이트
            Get<Image>(cursor + Button_Count).gameObject.SetActive(false);
        }   
    }
}