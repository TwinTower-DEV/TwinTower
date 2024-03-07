using System.Collections;
using UnityEngine.SceneManagement;

namespace TwinTower
{
    /// <summary>
    /// ScreenManager 클래스입니다.  게임 씬을 관리하는 클래스입니다.
    /// </summary>
    public class ScreenManager : Manager<ScreenManager>
    {
        public IEnumerator CurrentScreenReload()
        {
            yield return StartCoroutine(UI_ScreenFader.FadeScenOut());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield return StartCoroutine(UI_ScreenFader.FadeSceneIn());
        }
    }
}