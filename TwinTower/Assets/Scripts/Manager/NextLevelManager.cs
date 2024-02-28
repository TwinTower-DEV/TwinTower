using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 동시 접근을 막기 위한 메니저
/// 다음 단계로 가게 해줌.
/// GameManager로 옮겨도 될듯.
/// </summary>
namespace TwinTower
{
    public class NextLevelManager : Manager<NextLevelManager>
    {
        private bool isActive = false;

        public void NextLevel() {
            if (!isActive) {
                StartCoroutine(nextlevel());
            }
        }

        IEnumerator nextlevel() {
            isActive = true;
            Debug.Log("다음 단계 진입");
            yield return new WaitForSeconds(1f);
            isActive = false;
        }
    }  
}

