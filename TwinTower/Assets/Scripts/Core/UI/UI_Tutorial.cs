using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace TwinTower
{
    public class UI_Tutorial: UI_Base
    {
        enum Images
        {
            Button
        }

        enum Texts
        {
            Context
        }
        public override void Init()
        {
            Bind<Image>(typeof(Images));
            Bind<TextMeshProUGUI>(typeof(Texts));
            UIManager.Instance.InputHandler += KeyInput;
            Get<Image>((int)Images.Button).gameObject.SetActive(false);
            Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
            canvas.sortingOrder = 8;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
        }

        private void KeyInput()
        {
            if (!Input.anyKey)
                return;
            if (_uiNum != UIManager.Instance.UINum)
                return;
            if (UIManager.Instance.FadeCheck)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI_ClickSoundEffect();
                if (DataManager.Instance.UIGameDatavalue.langaugecursor == 0)
                    UIManager.Instance.ShowNormalUI<UI_Menu>();
                else
                    UIManager.Instance.ShowNormalUI<UI_Menu_ENG>();
                InputController.Instance.ReleaseControl();
                Time.timeScale = 0;
            }
        }

        public void SetText(string text)
        {
            Debug.Log(text);
            Get<TextMeshProUGUI>((int)Texts.Context).gameObject.GetComponent<TextMeshProUGUI>().text = text;
        }

        public void SetActives()
        {
            Get<Image>((int)Images.Button).gameObject.SetActive(true);
        }

        public void SetPosition(Vector3 pos, Vector3 offset)
        {
            gameObject.transform.position = pos + offset;
        }

        public void Close()
        {
            UIManager.Instance.InputHandler -= KeyInput;
            UIManager.Instance.CloseNormalUI(this);
        }
    }
}