using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 다음 단계에 진입해주는 매니저
/// Stair로부터 다음 Scene이름을 받아야 다음 단계 조건이 충족되면 그 씬을 불러와줌.
/// 또한, 다음 씬으로 이동과 동시에 GameManager의 Player가 변경되기 때문에 이와 함께 불러와줌.
/// </summary>
namespace TwinTower
{
    public class NextLevelManager : Manager<NextLevelManager>
    {
        private bool isActive = false;

        public void NextLevel(string nextScene) {
            if (!isActive) {
                StartCoroutine(nextlevel(nextScene));
            }
        }

        IEnumerator nextlevel(string nextScene) {
            isActive = true;
            SceneManager.LoadScene(nextScene);
            yield return new WaitForSeconds(1f);
            isActive = false;
            GameManager.Instance.FindPlayer();
        }
    }  
}

