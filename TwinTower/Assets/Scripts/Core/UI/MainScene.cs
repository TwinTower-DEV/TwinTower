using UnityEngine;

namespace TwinTower
{
    public class MainScene : UI_Base
    {
        public override void Init()
        {
            UIManager.Instance.ShowNormalUI<UI_MainScene>();
        }
    }
}