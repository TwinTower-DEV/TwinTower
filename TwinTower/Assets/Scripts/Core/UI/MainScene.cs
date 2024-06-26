using UnityEngine;

namespace TwinTower
{
    public class MainScene : UI_Base
    {
        public override void Init()
        {
            if (DataManager.Instance.UIGameDatavalue.langaugecursor == 0)
                UIManager.Instance.ShowNormalUI<UI_MainScene>();
            else
                UIManager.Instance.ShowNormalUI<UI_MainScene_ENG>();
        }
    }
}