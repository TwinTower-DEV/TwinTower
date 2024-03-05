using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TwinTower;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 철제상자, 나무상자를 담당하는 스크립트
/// 화살 피격, 밀기에 대한 내용이 담겨져 있다.
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
