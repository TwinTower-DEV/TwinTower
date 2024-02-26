using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 컨트롤에 관한 것입니다.
/// 플레이어 이동 시 moveToward를 사용할거면 코루틴 사용해야 할듯.
/// </summary>
public class PlayerController : MonoBehaviour {
    private Vector2 targetPosition;

    // Update is called once per frame

    void Awake() {
        targetPosition = transform.position;
    }
    void FixedUpdate()
    {   // 대각선 막는거 미구현
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 lookdirection = new Vector2(horizontal, vertical);
        
        if (targetPosition == (Vector2)transform.position) {                   // 멈춰있을 경우에만 이동 가능하게
            RaycastHit2D hit = Physics2D.Raycast(transform.position, lookdirection, 1.0f, LayerMask.GetMask("Wall"));
            if (hit.collider == null) {
                targetPosition = (Vector2)transform.position + lookdirection;
            }
        }
        else {      // 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 0.1f);
        }
    }
}
