using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기믹 발동 발판이다.
/// 아마 플레이어쪽에 추가되지 않을까 싶다.
/// 그러면 수정 필요
/// </summary>
public class PressurePlate : MonoBehaviour {
    public GameObject activateObject;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Box")) {
            ActivateObject active =  activateObject.GetComponent<ActivateObject>();
            active.Launch();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Box")) {
            ActivateObject active =  activateObject.GetComponent<ActivateObject>();
            active.UnLaunch();
        }
    }
}
