using System;
using System.Collections;
using System.Collections.Generic;
using TwinTower;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 무빙워크 되긴 함. 오류 개많음
/// 이거 플레이어 상자 정리 필요
/// 무빙워크로 원래 방향과 반대 되는 곳으로 이동할때 아예 이동 불가하게 막을 것인지 아닌지
/// 만약 이동 가능하다면 상자로 갔을때 다시 밀려나는 모션을 할것인지 아닌지
/// 상자하고 플레이어 고민 해야할듯.
/// </summary>
public class MovingWalk : MonoBehaviour {
    private void OnTriggerStay2D(Collider2D other) {
        MoveControl moveableObject = other.GetComponent<MoveControl>();
        if (moveableObject != null && moveableObject.MoveCheck(transform.up)) {     // 이동 가능할때
            moveableObject.DirectSetting(transform.up);  
            Debug.Log("실행 안된거임?");
        }
    }
}
