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
            Get<Image>((int)Images.Button).gameObject.SetActive(false);
            Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
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
            UIManager.Instance.CloseNormalUI(this);
        }
    }
}