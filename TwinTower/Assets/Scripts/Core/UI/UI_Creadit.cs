using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    public class UI_Creadit : UI_Base
    {

        private Animator _animator;
        public override void Init()
        {
            ManagerSet.UI.InputHandler -= KeyInput;
            ManagerSet.UI.InputHandler += KeyInput;

            StartCoroutine(end());
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
                UI_SoundEffect();
                ManagerSet.UI.CloseNormalUI(this);
            }
        }
        private IEnumerator end()
        {
            yield return new WaitForSeconds(14f);
            ManagerSet.UI.CloseNormalUI(this);
        }
    }
}