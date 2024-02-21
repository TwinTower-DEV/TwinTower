using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어의 컨트롤에 관한 것입니다.
/// </summary>
public class PlayerController : MonoBehaviour {
    private Rigidbody2D rigidbody2d;
    private Vector2 targetPosition;

    // Update is called once per frame

    void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        targetPosition = rigidbody2d.position;
    }
    void FixedUpdate()
    {   // 대각선 막는거 미구현
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 lookdirection = new Vector2(horizontal, vertical);
        
        if (targetPosition == rigidbody2d.position) {                   // 멈춰있을 경우에만 이동 가능하게
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position, lookdirection, 0.8f, LayerMask.GetMask("Wall"));
            if (hit.collider == null) {
                targetPosition = (Vector2)transform.position + lookdirection;
            }
        }
        else {      // 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 0.1f);
        }
    }
}
