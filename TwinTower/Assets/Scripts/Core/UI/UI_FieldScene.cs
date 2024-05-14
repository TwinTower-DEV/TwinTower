using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TwinTower
{
    public class UI_FieldScene : UI_Base
    {
        private int stage = 1;
        [SerializeField] private AudioClip BGM;
        enum Images
        {
            RestartButton,
            BackButton
        }

        enum Texts
        {
            FloorText
        }
        
        public override void Init()
        {
            SoundManager.Instance.ChangeBGM(BGM);

            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));
            
            Get<Image>((int)Images.RestartButton).gameObject.BindEvent(Restart, Define.UIEvent.Click);
            Get<Image>((int)Images.BackButton).gameObject.BindEvent(Setting, Define.UIEvent.Click);
            Get<TextMeshProUGUI>((int)Texts.FloorText).gameObject.GetComponent<TextMeshProUGUI>().text = "Floor " + SceneManager.GetActiveScene().buildIndex.ToString();

            UIManager.Instance.InputHandler -= KeyInPut;
            UIManager.Instance.InputHandler += KeyInPut;
        }
        
        private void KeyInPut()
        {
            if (!Input.anyKey)
                return;
            if (_uiNum != UIManager.Instance.UINum)
                return;
            if (UIManager.Instance.FadeCheck)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.ShowNormalUI<UI_Menu>();
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
            UIManager.Instance.ShowNormalUI<UI_SettingScene>();
        }

    }
}