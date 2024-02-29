using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 철제상자, 나무상자를 담당하는 스크립트
/// 화살 피격, 밀기에 대한 내용이 담겨져 있다.
/// </summary>
public class Box : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private int Health;
    private bool isMoving;
    
    // 입력 감지로 하는게 나을 듯 함. -> Player 입력 완성이 되어야 함.
    // 하고자 하는것 - 플레이어가 상자 주변에 있을 때 입력이 되는 것을 기반으로 이동 가능할지 안될지 결정
    // 플레이어가 주변에 있을 때 -> Layer를 wall로 변경 또는 이동 가능한 곳으로 변경. 이동 가능할 경우 동시 이동.
    // 회의때 얘기 필요.
    
    // 플레이어쪽에서 상자로 이동 가능한지 판별 -> bool형 반환으로 처리 -> true일경우 이동 가능 false일 경우 이동 불가능.
    public bool MoveBox(Vector3 lookdirection) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookdirection, 0.8f, LayerMask.GetMask("Wall"));
        if (hit.collider == null)
        {
            Vector2 targetPosition = transform.position + lookdirection;
            StartCoroutine(MoveToTargetPosition(targetPosition));
            return true;
        }
        return false;
    }

    IEnumerator MoveToTargetPosition(Vector3 targetPosition) {
        isMoving = true;
        while (transform.position != targetPosition) {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            yield return null;
        }

        // 이동이 완료된 후 마지막 위치 설정
        
        transform.position = (Vector3)TileFindManager.Instance.gettileCentorLocation(transform.position);
        isMoving = false;
    }

    // 일단 플레이어 위치를 계속해서 받아와 이동 가능한지 불가능 한지 판별하는 방법을 선택하였음.
    // 플레이어와 박스와의 거리가 1.2보다 작을 경우 플레이어 위치와 상자의 위치를 보고 이동 가능한지 확인 후 
    // 이동 불가능 할 경우 우선적으로 layer를 Wall로 변경 시키는 방법으로 선택하였음.
    // 이 방법 문제점 - 큐브의 위치가 조금 이상해짐.
    
    // layer = 0 -> default
    // layer = 6 -> wall
    void FixedUpdate() {
        /*Vector3 lookdirectionplayer1 = transform.position - Player1.transform.position;
        Vector3 lookdirectionplayer2 = transform.position - Player2.transform.position;

        if (lookdirectionplayer1.magnitude <= 1.2f) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookdirectionplayer1, 0.8f, LayerMask.GetMask("Wall"));
            if (hit.collider == null) {
                gameObject.layer = 0;
            }
            else gameObject.layer = 7;
        }
        else if (lookdirectionplayer2.magnitude <= 1.2f) {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookdirectionplayer2, 0.8f, LayerMask.GetMask("Wall"));
            if (hit.collider == null) {
                gameObject.layer = 0;
            }
            else gameObject.layer = 7;
        }
        else gameObject.layer = 0;*/
    }

    // Collistion 이용 방법
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            MoveBox(transform.position - other.transform.position);
        }
    }

    // 화살 피격 시 체력 감소 및 체력 없을 시 오브젝트 자체 삭제
    public void ReduceHealth() {
        Health--;
        if(Health <= 0) Destroy(gameObject);
    }

    public bool getMoving() {
        return isMoving;
    }
}
