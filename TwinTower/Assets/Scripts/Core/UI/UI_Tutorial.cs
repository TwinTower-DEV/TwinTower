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
            ManagerSet.UI.InputHandler += KeyInput;
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
            if (_uiNum != ManagerSet.UI.UINum)
                return;
            if (ManagerSet.UI.FadeCheck)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI_ClickSoundEffect();
                Close();
            }
        }

        public void SetText(string text)
        {
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
            ManagerSet.UI.InputHandler -= KeyInput;
            ManagerSet.UI.CloseNormalUI(this);
        }
    }
}