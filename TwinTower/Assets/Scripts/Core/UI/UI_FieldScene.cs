using UnityEngine;

namespace TwinTower
{
    public class UI_FieldScene : UI_Base
    {
        public override void Init()
        {
            UIManager.Instance.InputHandler -= KeyInPut;
            UIManager.Instance.InputHandler += KeyInPut;
        }
        
        private void KeyInPut()
        {
            if (!Input.anyKey)
                return;
            if (_uiNum != UIManager.Instance.UINum)
                return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.ShowNormalUI<UI_Menu>();
                InputController.Instance.ReleaseControl();
                Time.timeScale = 0;
            }
        }
        
    }
}