using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    public class UI_Clear : UI_Base
    {
        enum Texts
        {
            Stage
        }

        private Animator animator;
        public override void Init()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));
            Get<TextMeshProUGUI>((int)Texts.Stage).text = "0" + SceneManager.GetActiveScene().buildIndex.ToString()
                                                              + "\n" + "0" +
                                                              (SceneManager.GetActiveScene().buildIndex + 1).ToString();
            animator = Get<TextMeshProUGUI>((int)Texts.Stage).gameObject.GetComponent<Animator>();

            StartCoroutine(Animation());
        }

        private IEnumerator Animation()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsStart", true);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(ScreenManager.Instance.NextSceneload());
        }
    }
}