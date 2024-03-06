using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TwinTower;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// MoveControl을 상속 - 플레이어에게 밀어지는 경우 존재하기 때문에 상속 받음.
/// MoveCheck의 경우 상자는 뒤에 상자와 벽이 있으면 밀어지면 안되기 때문(플레이어는 상자가 있으면 가능)
/// </summary>
public class Box : MoveControl {
    [SerializeField] private int Health;

    public override bool MoveCheck(Vector3 movedir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + movedir * 0.5f , movedir, 0.5f, _layerMask);
        if (hit.collider == null) return true;

        return false;
    }
    

    // 화살 피격 시 체력 감소 및 체력 없을 시 오브젝트 자체 삭제
    public void ReduceHealth() {
        Health--;
        if(Health <= 0) Destroy(gameObject);
    }
}
