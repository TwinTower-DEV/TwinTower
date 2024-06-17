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
            UIManager.Instance.InputHandler -= KeyInput;
            UIManager.Instance.InputHandler += KeyInput;

            StartCoroutine(end());
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
                UIManager.Instance.CloseNormalUI(this);
            }
        }
        private IEnumerator end()
        {
            yield return new WaitForSeconds(14f);
            UIManager.Instance.CloseNormalUI(this);
        }
    }
}