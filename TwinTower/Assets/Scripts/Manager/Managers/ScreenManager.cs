using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    /// <summary>
    /// ScreenManager 클래스입니다.  게임 씬을 관리하는 클래스입니다.
    /// </summary>
    public class ScreenManager : Manager<ScreenManager>
    {
        private UI_ScreenFader fader;
        public override void Init()
        {
            fader = ResourceManager.Instance.Instantiate($"UI/FadeScean").GetComponent<UI_ScreenFader>();
        }

        public async UniTask CurrentScreenReload()
        {
            await UI_ScreenFader.Instance.FadeSceneOut();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            await UI_ScreenFader.Instance.FadeSceneIn();
            GameManager.Instance.FindPlayer();
        }

        public async UniTask Reload()
        {
            await CurrentScreenReload();
        }

        public async UniTask FadeInOut()
        {
            await UI_ScreenFader.Instance.FadeSceneOut();
            await UI_ScreenFader.Instance.FadeSceneIn();
        }
        public async UniTask NextSceneload(string s = null)
        {
            UIManager.Instance.Clear();
            UIManager.Instance.iscutSceenCheck = false;
            await UI_ScreenFader.Instance.FadeSceneOut();

            if (s == null)
            {
                if (SceneManager.GetActiveScene().buildIndex + 1 >= 15)
                {
                    InputManager.Destroys();
                    SceneManager.LoadScene("MainScene");
                }
                else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    DataManager.Instance.saveload.ChangeCurrSaveSlot(0);
                    DataManager.Instance.saveload.Save(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).name);
                }
            }
            else
                SceneManager.LoadScene(s);

            await UI_ScreenFader.Instance.FadeSceneIn();
            //GameManager.Instance.FindPlayer();
        }
    }
}