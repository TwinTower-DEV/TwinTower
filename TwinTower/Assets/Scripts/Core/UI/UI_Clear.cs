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
            Stage,
            Text,
            Clear
        }

        private Animator animator;
        public override void Init()
        {
            UIManager.Instance.isClearUICheck = true;
            Bind<TextMeshProUGUI>(typeof(Texts));
            Get<TextMeshProUGUI>((int)Texts.Clear).gameObject.SetActive(false);
            if(SceneManager.GetActiveScene().buildIndex < 10 && SceneManager.GetActiveScene().buildIndex + 1 < 10)
                Get<TextMeshProUGUI>((int)Texts.Stage).text = "0" + SceneManager.GetActiveScene().buildIndex.ToString()
                                                              + "\n" + "0" +
                                                              (SceneManager.GetActiveScene().buildIndex + 1).ToString();
            else if(SceneManager.GetActiveScene().buildIndex < 10 && SceneManager.GetActiveScene().buildIndex + 1 >= 10)
                Get<TextMeshProUGUI>((int)Texts.Stage).text = "0" + SceneManager.GetActiveScene().buildIndex.ToString()
                                                                  + "\n" + (SceneManager.GetActiveScene().buildIndex + 1).ToString();
            else if (SceneManager.GetActiveScene().buildIndex >= 10 &&
                     SceneManager.GetActiveScene().buildIndex + 1 >= 10)
            {
                if (SceneManager.GetActiveScene().buildIndex + 1 < 15)
                    Get<TextMeshProUGUI>((int)Texts.Stage).text = SceneManager.GetActiveScene().buildIndex.ToString()
                                                                  + "\n" +
                                                                  (SceneManager.GetActiveScene().buildIndex + 1)
                                                                  .ToString();
                else
                {
                    Get<TextMeshProUGUI>((int)Texts.Text).gameObject.SetActive(false);
                    Get<TextMeshProUGUI>((int)Texts.Stage).gameObject.SetActive(false);
                    Get<TextMeshProUGUI>((int)Texts.Clear).gameObject.SetActive(true);
                }
            }

            animator = Get<TextMeshProUGUI>((int)Texts.Stage).gameObject.GetComponent<Animator>();
            if (SceneManager.GetActiveScene().buildIndex + 1 < 15)
            {
                StartCoroutine(Animation());
            }
            else
            {
                StartCoroutine(End());
            }
        }

        private IEnumerator End()
        {
            yield return new WaitForSeconds(3.0f);
            StartCoroutine(ScreenManager.Instance.NextSceneload());
        }
        private IEnumerator Animation()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsStart", true);
            yield return new WaitForSeconds(1.5f);
            UIManager.Instance.isClearUICheck = false;
            StartCoroutine(ScreenManager.Instance.NextSceneload());
        }
    }
}