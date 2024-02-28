using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;

/// <summary>
/// 계단, 플레이어가 동시에 진입 시 다음 단계 진입.
/// OnPlayer로 플레이어가 계단 위에 있을 경우 다른 계단 확인 후 다음 단계 진입.
/// Manager 이용.
/// </summary>
public class Stair : MonoBehaviour
{
    private bool OnPlayer;
    public Stair stair2;

    private void Awake()
    {
        OnPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            OnPlayer = true;
            if (stair2.getOnPlayer() && OnPlayer) {                         // 다른 계단에 플레이어가 위치해 있는지 확인
                NextLevelManager.Instance.NextLevel();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) OnPlayer = false;
    }

    public bool getOnPlayer() {
        return OnPlayer;
    }
}

