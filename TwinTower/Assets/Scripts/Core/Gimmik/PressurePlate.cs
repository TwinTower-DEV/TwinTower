using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기믹 발동 발판이다.
/// 활성화 오브젝트와 연결된다.
/// </summary>

namespace TwinTower
{
    public class PressurePlate : MonoBehaviour
    {
        public GameObject activateObject;

        // 발판과 연결되어 있는 activateObject를 Launch시킴.(문 열기, 화살 발사.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
                other.gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                ActivateObject active = activateObject.GetComponent<ActivateObject>();
                active.Launch();
            }
        }

        // 발판과 연결되어 있는 activateObject를 UnLaunch시킴.(문 닫기)
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") ||
                other.gameObject.layer == LayerMask.NameToLayer("Box"))
            {
                ActivateObject active = activateObject.GetComponent<ActivateObject>();
                active.UnLaunch();
            }
        }
    }
}
