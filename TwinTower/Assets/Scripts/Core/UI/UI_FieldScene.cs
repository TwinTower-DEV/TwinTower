using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TwinTower
{
    public class UI_FieldScene : UI_Base
    {
        enum Images
        {
            RestartButton,
            BackButton
        }

        enum Texts
        {
            FloorText,
            CountText
        }
        
        public override void Init()
        {
            ManagerSet.Gamemanager.CurrentScnen(this);
            
            InputManager.Instance.UpDateCount();
            
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));
            
            Get<Image>((int)Images.RestartButton).gameObject.BindEvent(Restart, Define.UIEvent.Click);
            Get<Image>((int)Images.BackButton).gameObject.BindEvent(Setting, Define.UIEvent.Click);
            Get<TextMeshProUGUI>((int)Texts.FloorText).gameObject.GetComponent<TextMeshProUGUI>().text = "Floor " + SceneManager.GetActiveScene().buildIndex.ToString();

            ManagerSet.UI.InputHandler -= KeyInPut;
            ManagerSet.UI.InputHandler += KeyInPut;
        }
        
        private void KeyInPut()
        {
            if (!Input.anyKey)
                return;
            if (_uiNum != ManagerSet.UI.UINum)
                return;
            if (ManagerSet.UI.FadeCheck)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI_ClickSoundEffect();
                ManagerSet.UI.ShowNormalUI<UI_Menu>();
                InputController.Instance.ReleaseControl();
                Time.timeScale = 0;
            }
        }

        private void Restart()
        {
            InputController.Instance.ReleaseControl();
            StartCoroutine(ScreenManager.Instance.CurrentScreenReload());
        }

        private void Setting()
        {
            ManagerSet.UI.ShowNormalUI<UI_SettingScene>();
        }

        public void CountUpdate(int count)
        {
            Get<TextMeshProUGUI>((int)Texts.CountText).text = "\n\n" + count.ToString();
        }
    }
}